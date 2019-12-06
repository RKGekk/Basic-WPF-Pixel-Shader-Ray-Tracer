using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BasicRender.Engine {
    public class GameTimer {

        private const string lib = "KERNEL32";

        [DllImport(lib)]
        private static extern int QueryPerformanceCounter(ref Int64 count);

        [DllImport(lib)]
        private static extern int QueryPerformanceFrequency(ref Int64 frequency);

        private double _secondsPerCount;
        private double _deltaTime;

        private Int64 _baseTime;
        private Int64 _pausedTime;
        private Int64 _stopTime;
        private Int64 _prevousTime;
        private Int64 _currentTime;

        private bool _isStopped;

        public GameTimer() {
            _secondsPerCount = 0.0;
            _deltaTime = -1.0;
            _baseTime = 0;
            _pausedTime = 0;
            _stopTime = 0;
            _prevousTime = 0;
            _currentTime = 0;
            _isStopped = false;

            Int64 countPerSec = 0;
            QueryPerformanceFrequency(ref countPerSec);
            _secondsPerCount = 1.0 / ((double)countPerSec);
        }

        public float gameTime() {
            if (_isStopped) {
                return (float)(((_stopTime - _pausedTime) - _baseTime) * _secondsPerCount);
            }
            else {
                return (float)(((_currentTime - _pausedTime) - _baseTime) * _secondsPerCount);
            }
        }

        public float deltaTime() {
            return (float)_deltaTime;
        }

        public void reset() {
            QueryPerformanceCounter(ref _currentTime);
            _baseTime = _currentTime;
            _prevousTime = _currentTime;
            _stopTime = 0;
            _isStopped = false;
        }

        public void start() {
            QueryPerformanceCounter(ref _currentTime);
            if (_isStopped) {
                _pausedTime += (_currentTime - _stopTime);
                _prevousTime = _currentTime;
                _stopTime = 0;
                _isStopped = false;
            }
        }

        public void stop() {
            if (!_isStopped) {
                QueryPerformanceCounter(ref _currentTime);
                _stopTime = _currentTime;
                _isStopped = true;
            }
        }

        public void tick() {
            if (_isStopped) {
                _deltaTime = 0.0;
                return;
            }
            QueryPerformanceCounter(ref _currentTime);
            _deltaTime = (_currentTime - _prevousTime) * _secondsPerCount;
            _prevousTime = _currentTime;
            if (_deltaTime < 0.0) {
                _deltaTime = 0.0;
            }
        }
    }
}
