using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualPianoLiar
{
    public class Pauses: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int _ShortPause;

        int _LongPauseStar;
        int _TooLongPauseStar;
        int _BrackestPauseStar;
        int _LinePauseStar;

        public int ShortPause
        {
            get { return _ShortPause; }
            set
            {
                _ShortPause = value;
                RaisePropertyChanged("LongPause");
                RaisePropertyChanged("TooLongPause");
                RaisePropertyChanged("BrackestPause");
                RaisePropertyChanged("LinePause");
            }
        }
        public int LongPause
        {
            get { return ShortPause * _LongPauseStar; }
        }
        public int TooLongPause
        {
            get { return ShortPause * _TooLongPauseStar; }
        }
        public int BrackestPause
        {
            get { return ShortPause * _BrackestPauseStar; }
        }
        public int LinePause
        {
            get { return ShortPause * _LinePauseStar; }
        }

        public int LongPauseStar
        {
            get { return _LongPauseStar; }
            set
            {
                _LongPauseStar = value;
                RaisePropertyChanged("LongPause");
            }
        }
        public int TooLongPauseStar
        {
            get { return _TooLongPauseStar; }
            set
            {
                _TooLongPauseStar = value;
                RaisePropertyChanged("TooLongPause");
            }
        }
        public int BrackestPauseStar
        {
            get { return _BrackestPauseStar; }
            set
            {
                _BrackestPauseStar = value;
                RaisePropertyChanged("BrackestPause");
            }
        }
        public int LinePauseStar
        {
            get { return _LinePauseStar; }
            set
            {
                _LinePauseStar = value;
                RaisePropertyChanged("LinePause");
            }
        }

        public Pauses()
        {
            _ShortPause = 80;

            _LongPauseStar = 2;
            _TooLongPauseStar = 4;
            _BrackestPauseStar = 4;
            _LinePauseStar = 5;
        }

        public void SetByDefault()
        {
            _ShortPause = 80;

            _LongPauseStar = 2;
            _TooLongPauseStar = 4;
            _BrackestPauseStar = 4;
            _LinePauseStar = 5;

            RaisePropertyChanged("LongPause");
            RaisePropertyChanged("TooLongPause");
            RaisePropertyChanged("BrackestPause");
            RaisePropertyChanged("LinePause");
        }

        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
