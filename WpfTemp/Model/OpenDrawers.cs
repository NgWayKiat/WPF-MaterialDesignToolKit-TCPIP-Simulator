using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTemp.Model
{
    public class OpenDrawers : ViewModelBase
    {
        private bool _LeftDrawer;

        private bool _RightDrawer;

        private bool _TopDrawer;

        private bool _BottomDrawer;

        public bool LeftDrawer
        {
            get
            {
                return _LeftDrawer;
            }
            set
            {
                SetProperty(ref _LeftDrawer, value, "LeftDrawer");
            }
        }

        public bool RightDrawer
        {
            get
            {
                return _RightDrawer;
            }
            set
            {
                SetProperty(ref _RightDrawer, value, "RightDrawer");
            }
        }

        public bool TopDrawer
        {
            get
            {
                return _TopDrawer;
            }
            set
            {
                SetProperty(ref _TopDrawer, value, "TopDrawer");
            }
        }

        public bool BottomDrawer
        {
            get
            {
                return _BottomDrawer;
            }
            set
            {
                SetProperty(ref _BottomDrawer, value, "BottomDrawer");
            }
        }
    }
}
