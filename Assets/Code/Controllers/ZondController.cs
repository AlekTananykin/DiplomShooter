using Assets.Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Controllers
{
    internal class ZondController : IInitialization, ICleanup
    {
        GameObject _cameraView;
        GameObject _zondView;
        GameObject _heroView;
        public ZondController(GameObject frontCameraView, 
            GameObject zondView, GameObject heroView)
        {
            _cameraView = frontCameraView;
            _zondView = zondView;
            _zondView.layer = LayerMask.NameToLayer("Ignore Raycast");
            _heroView = heroView;
        }

        public void Initialize()
        {
            ZondScript zondScript = _zondView.AddComponent<ZondScript>();
            zondScript.On_Zond_Trigger_Enter += SetCameraCarrier;
            zondScript.On_Zond_Trigger_Exit += UnsetCameraCarrier;
            SetCameraCarrier(_heroView);
        }

        public void Cleanup()
        {
            //ZondScript zondScript = _zondView.GetComponent<ZondScript>();
            //zondScript.On_Zond_Trigger_Enter -= SetCameraCarrier;
            //zondScript.On_Zond_Trigger_Exit -= UnsetCameraCarrier;
        }

        private void UnsetCameraCarrier(GameObject cameraCarrier)
        {
            SetCameraCarrier(_heroView);
        }

        private void SetCameraCarrier(GameObject cameraCarrier)
        {
            _cameraView.transform.SetParent(cameraCarrier.transform, true);
            
            Collider carierCollider = cameraCarrier.GetComponent<Collider>();
            _cameraView.transform.localPosition = new Vector3(0f,
                carierCollider.bounds.size.y / 2f, carierCollider.bounds.size.y * 2f);
            _cameraView.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        public void SetPosition(Vector3 position)
        {
            _zondView.transform.position = position;
        }

    }
}
