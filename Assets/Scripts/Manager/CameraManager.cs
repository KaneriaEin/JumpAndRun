using Cinemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Manager
{
    class CameraManager : Singleton<CameraManager>
    {
        public CameraController cameraController;


        public void ScreenShake()
        {
            cameraController.ScreenShake();
        }
    }
}
