using System.Collections;
using TMPro;
using UnityEngine;

namespace VSX.UniversalVehicleCombat
{
    public class DamageLabel : MonoBehaviour
    {
        // Reference to the damage text, font size, critical hit font size, and color
        [Header("Damage Label")]
        [SerializeField] private TMP_Text damageText;  // Text object to display the damage amount
        [SerializeField] private float normalFontSize = 42;  // Standard font size
        [SerializeField] private float critFontSize = 52;    // Font size for critical hits
        [SerializeField] private Color normalFontColor = Color.white;  // Standard font color

        // Percentage of the display duration at which the text will start fading out
        [SerializeField] private float startColorFadeAtPercent = 0.8f;

        // Animation curve for easing the animation of the label's movement
        [Header("Animation easing")]
        [SerializeField] private AnimationCurve easeCurve;
        private float _displayDuration;  // Total duration of the label's display

        // Bezier curve settings for movement
        [Header("Bezier curve settings")]
        [SerializeField] private Vector2 highPointOffset = new Vector2(-350, 300); // Offset for the highest point in the curve
        [SerializeField] private Vector2 lowPointOffset = new Vector2(-100, -500); // Offset for the drop point in the curve
        [SerializeField] private float heightVariationMax = 150;  // Maximum variation in height for curve randomization
        [SerializeField] private float heightVariationMin = 50;   // Minimum variation in height

        // Direction-based animation offset adjustments
        private Vector3 _highPointOffsetBasedOnDirection = Vector3.zero;
        private Vector3 _dropPointOffsetBasedOnDirection = Vector3.zero;
        private bool _direction = true;  // Direction flag for the animation

        // Visualization settings for debugging the animation path in the editor
        [Header("Visualize")]
        [SerializeField] private bool displayGizmos;  // Toggle to display gizmos in the editor
        [SerializeField, Range(1, 30)] private int gizmoResolution = 20;  // Resolution for the gizmos
        private Vector3 _startingPositionForVisualization = Vector3.zero;  // Start position for the gizmo display

        // Reference to the pool manager to return labels to the pool after displaying
        private SpawnsDamagePopups _poolManager;

        // Coroutine to handle the movement animation
        private Coroutine _moveCoroutine;

        // Displays the Bezier curve path in the editor using gizmos (debugging visualization)
        private void OnDrawGizmos()
        {
            if (!displayGizmos)
                return;

            OrientCurveBasedOnDirection();

            Vector3 start = transform.position;

            if (Application.isPlaying)
                start = _startingPositionForVisualization;

            var heightVariation = heightVariationMax - heightVariationMin;

            Vector3 highPoint = start + _highPointOffsetBasedOnDirection + new Vector3(0, heightVariation, 0);
            Vector3 dropPoint = highPoint + _dropPointOffsetBasedOnDirection;
            int colorChangeIndex = (int)(startColorFadeAtPercent * gizmoResolution);

            Gizmos.color = Color.red;

            Vector3 prevPoint = start;

            for (int i = 1; i <= gizmoResolution; i++)
            {
                float time = i / (float)gizmoResolution;
                Vector3 nextPoint = CalculateBezierPoint(time, start, highPoint, dropPoint);

                if (i >= colorChangeIndex)
                    Gizmos.color = Color.yellow;

                Gizmos.DrawLine(prevPoint, nextPoint);
                prevPoint = nextPoint;
            }
        }

        // Adjusts the curve's orientation based on the direction flag
        private void OrientCurveBasedOnDirection()
        {
            _highPointOffsetBasedOnDirection = highPointOffset;
            _dropPointOffsetBasedOnDirection = lowPointOffset;

            if (_direction)
                return;

            // Reverse offsets if direction is false
            _highPointOffsetBasedOnDirection.x = -_highPointOffsetBasedOnDirection.x;
            _dropPointOffsetBasedOnDirection.x = -_dropPointOffsetBasedOnDirection.x;
        }

        // Calculates a point on the Bezier curve given start, control, and end points
        private Vector3 CalculateBezierPoint(float progress, Vector3 start, Vector3 control, Vector3 end)
        {
            float remainingPath = 1 - progress;
            Vector3 currentLocation = remainingPath * remainingPath * start;
            currentLocation += 2 * remainingPath * progress * control;
            currentLocation += progress * progress * end;

            return currentLocation;
        }

        // Initializes the label with duration and pool manager reference
        public void Initialize(float displayDuration, SpawnsDamagePopups poolManager)
        {
            _poolManager = poolManager;
            _displayDuration = displayDuration;

            OrientCurveBasedOnDirection();
        }

        // Configures and displays the damage label at the specified position
        public void Display(int damage, Vector3 objPosition, bool direction, bool isCrit)
        {
            transform.position = objPosition;
            _startingPositionForVisualization = objPosition;
            _direction = direction;

            // Set the text, color, and font size based on whether it's a critical hit
            damageText.SetText(damage.ToString());
            damageText.color = normalFontColor;
            damageText.enableVertexGradient = isCrit;
            damageText.fontSize = isCrit ? critFontSize : normalFontSize;

            if (_moveCoroutine != null)
                StopCoroutine(_moveCoroutine);

            _moveCoroutine = StartCoroutine(Move());
            StartCoroutine(ReturnDamageLabelToPool(_displayDuration));
        }

        // Coroutine to move the label along a Bezier curve path and fade out over time
        private IEnumerator Move()
        {
            float time = 0;
            float fadeStartTime = startColorFadeAtPercent * _displayDuration;

            OrientCurveBasedOnDirection();

            Vector3 start = transform.position;

            var heightVariation = Random.Range(heightVariationMin, heightVariationMax);
            Vector3 variation = new Vector3(0, heightVariation, 0);

            Vector3 highPoint = (start + _highPointOffsetBasedOnDirection + variation);
            Vector3 dropPoint = highPoint + _dropPointOffsetBasedOnDirection;

            while (time < _displayDuration)
            {
                time += Time.deltaTime;

                float progess = time / _displayDuration;
                float easedTime = easeCurve.Evaluate(progess);

                if (time > fadeStartTime)
                {
                    // Fade out text by reducing the alpha channel
                    Color color = damageText.color;
                    float newAlpha = Mathf.Lerp(1, 0, (time - fadeStartTime) / (_displayDuration - fadeStartTime));
                    color.a = newAlpha;
                    damageText.color = color;
                }

                transform.position = CalculateBezierPoint(easedTime, start, highPoint, dropPoint);

                yield return null;
            }
        }

        // Coroutine to return the label to the pool after a specified display duration
        private IEnumerator ReturnDamageLabelToPool(float displayLength)
        {
            yield return new WaitForSeconds(displayLength);
            _poolManager.ReturnDamageLabelToPool(this);
        }
    }
}
