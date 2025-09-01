using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG_BlocksEngine2.Environment
{
    public class BE2_TargetObject : MonoBehaviour, I_BE2_TargetObject
    {
        public Transform Transform => transform;
        public I_BE2_ProgrammingEnv ProgrammingEnv { get; set; }

        // Karakterin varsayılan (default) başlangıç pozisyonu
        public Vector3 defaultPosition;

        private void Awake()
        {
            // Sahnedeki ilk pozisyonu default olarak ayarla
            defaultPosition = transform.position;
        }

        // Başlangıç pozisyonunu değiştirmek için kullanılabilir
        public void SetStartPosition(Vector3 newStartPos)
        {
            transform.position = newStartPos;
            defaultPosition = newStartPos;
        }
    }
}
