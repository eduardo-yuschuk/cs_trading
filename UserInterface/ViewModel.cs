/*
   Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*/

using System.Collections.Generic;
using System.ComponentModel;

namespace UserInterface
{
    class ViewModel : INotifyPropertyChanged
    {
        private List<INode> _nodes = new List<INode>();

        public ViewModel()
        {
        }

        public List<INode> Nodes
        {
            get { return _nodes; }
            set
            {
                _nodes = value;
                NotifiyPropertyChanged("Nodes");
            }
        }

        void NotifiyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}