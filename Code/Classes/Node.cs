using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Inhuman
{
    public class Node : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                    UpdateTime();
                }
            }
        }

        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        private string _info;
        public string Info
        {
            get
            {
                return _info;
            }
            set
            {
                if (value != _info)
                {
                    _info = value;
                    NotifyPropertyChanged("Info");
                }
            }
        }

        private DateTime _created;
        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                if (value != _created)
                {
                    _created = value;
                    NotifyPropertyChanged("Created");
                }
            }
        }

        private DateTime _updated;
        public DateTime Updated
        {
            get
            {
                return _updated;
            }
            set
            {
                if (value != _updated)
                {
                    _updated = value;
                    NotifyPropertyChanged("Updated");
                }
            }
        }
	
	    //===================================================================================================================================================//
        public Node() : this(""){}
        
        //===================================================================================================================================================//
        public Node(string name)
	    {
		    Name = name;
            Id = System.Guid.NewGuid().ToString();
            Created = DateTime.Now;
            Updated = Created;
	    }

        //===================================================================================================================================================//
        public void UpdateTime()
        {
            Updated = DateTime.Now;
        }

        //===================================================================================================================================================//
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
