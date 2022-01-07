using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArkhamGamePlay
{
    public class ArrowControl : MonoBehaviour
    {
        [SerializeField] ArrowRenderer arrow;

        private void Start()
        {
            arrow.SetPositions(transform.position, AllComponents.Table.LocationZones[1].ZoneBehaviour.transform.position);
        }
    }
}