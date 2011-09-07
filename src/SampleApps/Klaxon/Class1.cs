using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CityIndexTools
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class Sounds
    {


        public void Exclamation()
        {
            System.Media.SystemSounds.Exclamation.Play();
        }
        public void Asterisk()
        {
            System.Media.SystemSounds.Asterisk.Play();
            
        }
        public void Beep()
        {
            System.Media.SystemSounds.Beep.Play();
            
        }
        public void Hand()
        {
            System.Media.SystemSounds.Hand.Play();
        }
        public void Question()
        {
            System.Media.SystemSounds.Question.Play();
        }
    }
}
