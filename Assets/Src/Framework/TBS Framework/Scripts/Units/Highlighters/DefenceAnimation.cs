using System.Collections;
using TbsFramework.Grid;
using UnityEngine;

namespace TbsFramework.Units.Highlighters
{
    public class DefenceAnimation : UnitHighlighter
    {
        [SerializeField] private float _magnitude = 1;

        private Vector3 _originalPosition;
        private Coroutine _coroutine;

        private void OnEnable() => CellGrid.Instance.TurnEnded += OnTurnEnded;
        private void OnDisable() => CellGrid.Instance.TurnEnded += OnTurnEnded;

        public override void Apply(Unit unit, Unit otherUnit)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                unit.transform.localPosition = _originalPosition;
            }

            _coroutine = StartCoroutine(DefenceAnimationCoroutine(unit));
        }

        private IEnumerator DefenceAnimationCoroutine(Unit unit)
        {
            var StartingPosition = unit.transform.position;
            _originalPosition = unit.transform.localPosition;
            var rnd = new System.Random();

            for (int i = 0; i < 5; i++)
            {
                var heading = new Vector3(((float)rnd.NextDouble() - 0.5f), (float)rnd.NextDouble() - 0.5f, 0);
                var direction = (heading / heading.magnitude) * _magnitude;
                float startTime = Time.time;

                while (startTime + 0.05f > Time.time)
                {
                    unit.transform.position = Vector3.Lerp(transform.position,
                        transform.position + direction,
                        ((startTime + 0.05f) - Time.time));
                    yield return 0;
                }

                startTime = Time.time;
                while (startTime + 0.05f > Time.time)
                {
                    unit.transform.position = Vector3.Lerp(transform.position,
                        transform.position - direction,
                        ((startTime + 0.05f) - Time.time));
                    yield return 0;
                }
            }

            unit.transform.position = StartingPosition;
            _originalPosition = unit.transform.localPosition;
        }

        private void OnTurnEnded(object sender, bool something)
        {
            _coroutine = null;
            _originalPosition = Vector3.zero;
        }
    }
}