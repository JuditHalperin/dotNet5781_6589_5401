﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace dotNET5781_01_6589_5401
{
    class Bus
    {
        static private Random rand = new Random(DateTime.Now.Millisecond);

        private string id;
        public string Id
        {
            get { return id; }
        }

        private DateTime dateOfBegining;
        public DateTime DateOfBegining
        {
            get { return dateOfBegining; }
            private set
            {
                if (value <= DateTime.Now)
                    dateOfBegining = value;

                else
                    Console.WriteLine("Invalid date");
            }
        }

        private DateTime dateOfLastTreat;
        public DateTime DateOfLastTreat
        {
            get { return dateOfLastTreat; }
            private set { dateOfLastTreat = value; }
        }

        private float totalKm;
        public float TotalKm
        {
            get { return totalKm; }
            private set { totalKm = value; }
        }

        private float kmSinceFueling;
        public float KmSinceFueled
        {
            get { return kmSinceFueling; }
            private set { kmSinceFueling = value; }
        }

        private float kmSinceTreated;
        public float KmSinceTreated
        {
            get { return kmSinceFueling; }
            private set { kmSinceFueling = value; }
        }

        public Bus(DateTime date, string id,out string msg) // constructor
        {
            DateOfBegining = date;
            DateOfLastTreat = date;
            this.checkId(id,out msg);
            TotalKm = 0;
            KmSinceFueled = 0;
            KmSinceTreated = 0;
        }

        private bool checkId(string value, out string msg)
        {

            if (value.Length < 7 || value.Length > 8) // wrong length
            {
                msg = "the id is bigger or smaller then what is needed";
                return false; // exeption...
            }

            int num;
            bool flag = int.TryParse(value, out num);
            if (!flag) // convert failed
            {
                msg = "the id was not number";
                return false;
            }

            if (num < 1000000 || num > 100000000)  // not all digits
            {
                msg = "the id was not just from digits";
                return false;
            }

            string tmp = Convert.ToString(num);

            if (dateOfBegining.Year < 2018 && tmp.Length == 7) // 7 digits
            {
                tmp = tmp.Insert(2, "-");
                tmp = tmp.Insert(6, "-");
            }

            else if (dateOfBegining.Year > 2018 && tmp.Length == 8) // 8 digits
            {
                tmp = tmp.Insert(3, "-");
                tmp = tmp.Insert(6, "-");
            }

            else // length does not fit the year
            {
                msg = "length does not fit the year";
                return false;
            }

            id = tmp;
            msg = "The bus was successfully inserted!";
            return true;
        }

        public void drive() // if possible - drive the bus
        {
            string msg;
            float km = kmOfDrive();
            bool possible = isValid(out msg, km);

            if (possible)
                updateKm(km);
            else
                Console.WriteLine("The bus cannot drive: " + msg);
        }

        private float kmOfDrive() // random length of drive
        {
            return rand.Next(0, 1200);
        }

        public bool isValid(out string msg, float km = 0) // test if the bus can drive
        {
            TimeSpan timeSinceLastTreat = DateTime.Now - DateOfLastTreat;
            if (timeSinceLastTreat.TotalDays > 365)
            {
                msg = "the bus needs a traet because the last one was a year ago";
                return false;
            }

            if (KmSinceTreated + km > 20000)
            {
                msg = "the bus needs a traet because it drived more than 20,000 km";
                return false;
            }

            if (KmSinceFueled + km > 1200)
            {
                msg = "the bus needs to get fueled";
                return false;
            }

            msg = "everything's alright";
            return true;

        }
        /// <summary>
        /// update the km fields after drive
        /// </summary>
        /// <param name="km"><additinal km that the bus drive>
        private void updateKm(float km)
        {
            TotalKm += km;
            KmSinceFueled += km;
            KmSinceTreated += km;
        }
        /// <summary>
        /// reset the km since last fuel.
        /// </summary>
        public void fuel()
        {
            KmSinceFueled = 0;
        }
        /// <summary>
        /// reset the km since last treat.
        /// </summary>
        public void treat()
        {
            KmSinceTreated = 0;
        }

    }
}
