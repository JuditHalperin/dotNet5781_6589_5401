﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DLAPI;
using DO;

namespace DL
{
    public sealed class DalXml : IDAL
    {
        #region singelton

        static readonly DalXml instance = new DalXml();
        public static DalXml Instance => instance;
        static DalXml() { }
        DalXml() { }

        #endregion

        #region DS XML Files

        string usersPath = @"UsersXml.xml"; // XElement
        string busesPath = @"BusesXml.xml"; // XMLSerializer
        string linesPath = @"LinesXml.xml"; // XMLSerializer
        string stationsPath = @"StationsXml.xml"; // XMLSerializer
        string lineStationsPath = @"LineStationsXml.xml"; // XMLSerializer
        string followingStationsPath = @"FollowingStationsXml.xml"; // XMLSerializer
        string drivingLinesPath = @"DrivingLinesXml.xml"; // XMLSerializer
        string managingCodePath = @"ManagingCodeXml.xml"; // XMLSerializer

        #endregion

        #region Users

        public void addUser(User user)
        {
            XElement rootElem = XMLTools.LoadListFromXMLElement(usersPath);

            XElement item = (from i in rootElem.Elements()
                             where i.Element("Username").Value == user.Username
                             select i).FirstOrDefault();

            if (item != null)
                throw new UserException("The user already exists.");

            rootElem.Add(new XElement("User",
                                   new XElement("Username", user.Username),
                                   new XElement("Password", user.Password),
                                   new XElement("IsManager", user.IsManager)));

            XMLTools.SaveListToXMLElement(rootElem, usersPath);            
        }
        public void removeUser(User user)
        {
            XElement rootElem = XMLTools.LoadListFromXMLElement(usersPath);

            XElement item = (from i in rootElem.Elements()
                             where i.Element("Username").Value == user.Username
                             select i).FirstOrDefault();

            if (item != null)
            {
                item.Remove();
                XMLTools.SaveListToXMLElement(rootElem, usersPath);
            }
            else
                throw new UserException("The user does not exist.");
        }
        public void updateUser(User user)
        {
            removeUser(getUser(user.Username));
            addUser(user);            
        }
        public User getUser(string username)
        {
            User item = (from i in XMLTools.LoadListFromXMLElement(usersPath).Elements()
                         where i.Element("Username").Value == username
                         select new User()
                         {
                             Username = i.Element("Username").Value,
                             Password = i.Element("Password").Value,
                             IsManager = bool.Parse(i.Element("IsManager").Value)
                         }).FirstOrDefault();

            if (item == null)
                throw new UserException("The user does not exist.");

            return item;
        }
        public IEnumerable<User> GetUsers()
        {
            return from i in XMLTools.LoadListFromXMLElement(usersPath).Elements()
                   select new User()
                   {
                       Username = i.Element("Username").Value,
                       Password = i.Element("Password").Value,
                       IsManager = bool.Parse(i.Element("IsManager").Value)
                   };
        }
        public IEnumerable<User> GetUsers(Predicate<User> condition)
        {
            return from i in XMLTools.LoadListFromXMLElement(usersPath).Elements()
                   let item = new User()
                   {
                       Username = i.Element("Username").Value,
                       Password = i.Element("Password").Value,
                       IsManager = bool.Parse(i.Element("IsManager").Value)
                   }
                   where condition(item)
                   select item;
        }

        #endregion

        #region Buses

        public void addBus(Bus bus)
        {
            if (DataSource.Buses.Exists(item => item.LicensePlate == bus.LicensePlate))
                throw new BusException("The bus already exists.");
            DataSource.Buses.Add(bus.Clone());
        }
        public void removeBus(Bus bus)
        {
            XElement rootElem = XMLTools.LoadListFromXMLElement(busesPath);

            XElement item = (from i in rootElem.Elements()
                             where i.Element("LicensePlate").Value == bus.LicensePlate
                             select i).FirstOrDefault();

            if (item != null)
            {
                item.Remove();
                XMLTools.SaveListToXMLElement(rootElem, busesPath);
            }
            else
                throw new BusException("The bus does not exist.");
        }
        public void updateBus(Bus bus)
        {
            Bus b = DataSource.Buses.Find(item => item.LicensePlate == bus.LicensePlate);
            if (b == null)
                throw new BusException("The bus does not exist.");
            DataSource.Buses.Remove(b); // remove the old bus
            DataSource.Buses.Add(bus.Clone()); // add the updated bus            
        }
        public Bus getBus(string licensePlate)
        {
            Bus bus = DataSource.Buses.Find(item => item.LicensePlate == licensePlate);
            if (bus == null)
                return null;
            return bus.Clone();
        }
        public IEnumerable<Bus> GetBuses()
        {
            return from item in DataSource.Buses
                   select item.Clone();
        }
        public IEnumerable<Bus> GetBuses(Predicate<Bus> condition)
        {
            return from item in DataSource.Buses
                   where condition(item)
                   select item.Clone();
        }

        #endregion

        #region Lines

        public int addLine(Line line)
        {
            line.ThisSerial = DataSource.serial++;
            DataSource.Lines.Add(line.Clone());
            return line.ThisSerial;
        }
        public void removeLine(Line line)
        {
            XElement rootElem = XMLTools.LoadListFromXMLElement(linesPath);

            XElement item = (from i in rootElem.Elements()
                             where int.Parse(i.Element("ThisSerial").Value) == line.ThisSerial
                             select i).FirstOrDefault();

            if (item != null)
            {
                item.Remove();
                XMLTools.SaveListToXMLElement(rootElem, linesPath);
            }
            else
                throw new LineException("The line does not exist.");
        }
        public void updateLine(Line line)
        {
            Line l = DataSource.Lines.Find(item => item.ThisSerial == line.ThisSerial);
            if (l == null)
                throw new LineException("The line does not exist.");
            DataSource.Lines.Remove(l); // remove the old line
            DataSource.Lines.Add(line.Clone()); // add the updated line  
        }
        public Line getLine(int serial)
        {
            return DataSource.Lines.Find(item => item.ThisSerial == serial).Clone();
        }
        public IEnumerable<Line> GetLines()
        {
            return (from item in DataSource.Lines
                    select item.Clone()).OrderBy(item => item.NumberLine);
        }
        public IEnumerable<Line> GetLines(Predicate<Line> condition)
        {
            return (from item in DataSource.Lines
                    where condition(item)
                    select item.Clone()).OrderBy(item => item.NumberLine);
        }
        public int countLines()
        {
            return DataSource.Lines.Count();
        }

        #endregion

        #region Stations

        public void addStation(Station station)
        {
            if (DataSource.Stations.Exists(item => item.ID == station.ID))
                throw new StationException("The station already exists.");
            DataSource.Stations.Add(station.Clone());
        }
        public void removeStation(Station station)
        {
            XElement rootElem = XMLTools.LoadListFromXMLElement(stationsPath);

            XElement item = (from i in rootElem.Elements()
                             where int.Parse(i.Element("ID").Value) == station.ID
                             select i).FirstOrDefault();

            if (item != null)
            {
                item.Remove();
                XMLTools.SaveListToXMLElement(rootElem, stationsPath);
            }
            else
                throw new StationException("The station does not exist.");
        }
        public void updateStation(Station station)
        {
            Station s = DataSource.Stations.Find(item => item.ID == station.ID);
            if (s == null)
                throw new LineException("The station does not exist.");
            DataSource.Stations.Remove(s); // remove the old station
            DataSource.Stations.Add(station.Clone()); // add the updated station
        }
        public Station getStation(int id)
        {
            Station station = DataSource.Stations.Find(item => item.ID == id);
            if (station == null)
                return null;
            return station.Clone();
        }
        public IEnumerable<Station> GetStations()
        {
            return (from item in DataSource.Stations
                    select item.Clone()).OrderBy(item => item.ID);
        }
        public IEnumerable<Station> GetStations(Predicate<Station> condition)
        {
            return (from item in DataSource.Stations
                    where condition(item)
                    select item.Clone()).OrderBy(item => item.ID); ;
        }
        public int countStations()
        {
            return DataSource.Stations.Count();
        }

        #endregion

        #region LineStations

        public void addLineStation(LineStation lineStation)
        {
            try // check if the station exists
            {
                getStation(lineStation.ID);
            }
            catch (StationException ex)
            {
                throw new StationException(ex.Message);
            }
            if (DataSource.LineStations.Exists(item => item.NumberLine == lineStation.NumberLine && item.ID == lineStation.ID))
                throw new StationException("The line station already exists.");
            DataSource.LineStations.Add(lineStation.Clone());
        }
        public void removeLineStation(LineStation lineStation)
        {
            XElement rootElem = XMLTools.LoadListFromXMLElement(lineStationsPath);

            XElement item = (from i in rootElem.Elements()
                             where int.Parse(i.Element("NumberLine").Value) == lineStation.NumberLine && int.Parse(i.Element("ID").Value) == lineStation.ID
                             select i).FirstOrDefault();

            if (item != null)
            {
                item.Remove();
                XMLTools.SaveListToXMLElement(rootElem, lineStationsPath);
            }
            else
                throw new StationException("The line station does not exist.");           
        }
        public void updateLineStation(LineStation lineStation)
        {
            removeLineStation(lineStation); // remove the old line station=delete according the key.
            DataSource.LineStations.Add(lineStation.Clone()); // add the updated line station
        }
        public LineStation getLineStation(int numberLine, int id)
        {
            LineStation lineStation = DataSource.LineStations.Find(item => item.NumberLine == numberLine && item.ID == id);
            if (lineStation == null)
                return null;
            return lineStation.Clone();
        }
        public IEnumerable<LineStation> GetLineStations()
        {
            return from item in DataSource.LineStations
                   select item.Clone();
        }
        public IEnumerable<LineStation> GetLineStations(Predicate<LineStation> condition)
        {
            return (from item in DataSource.LineStations
                    where condition(item)
                    select item.Clone()).OrderBy(item => item.PathIndex);
        }

        #endregion

        #region FollowingStations

        public void addTwoFollowingStations(TwoFollowingStations twoFollowingStations)
        {
            if (!DataSource.Stations.Exists(item => item.ID == twoFollowingStations.FirstStationID) || !DataSource.Stations.Exists(item => item.ID == twoFollowingStations.SecondStationID))
                throw new StationException("At least one of the station does not exist.");
            if (twoFollowingStations.FirstStationID == twoFollowingStations.SecondStationID)
                throw new StationException("Two identical stations.");
            if (DataSource.FollowingStations.Exists(item => (item.FirstStationID == twoFollowingStations.FirstStationID && item.SecondStationID == twoFollowingStations.SecondStationID) || (item.FirstStationID == twoFollowingStations.SecondStationID && item.SecondStationID == twoFollowingStations.FirstStationID)))
                throw new StationException("The two following stations already exist.");
            DataSource.FollowingStations.Add(twoFollowingStations.Clone());
        }
        public void removeTwoFollowingStations(TwoFollowingStations twoFollowingStations)
        {
            XElement rootElem = XMLTools.LoadListFromXMLElement(followingStationsPath);

            XElement item = (from i in rootElem.Elements()
                             where int.Parse(i.Element("FirstStationID").Value) == twoFollowingStations.FirstStationID && int.Parse(i.Element("SecondStationID").Value) == twoFollowingStations.SecondStationID
                             select i).FirstOrDefault();

            if (item != null)
            {
                item.Remove();
                XMLTools.SaveListToXMLElement(rootElem, followingStationsPath);
            }
            else
                throw new StationException("The two following stations do not exist.");
        }
        public void updateTwoFollowingStations(TwoFollowingStations twoFollowingStations)
        {
            TwoFollowingStations f = DataSource.FollowingStations.Find(item => (item.FirstStationID == twoFollowingStations.FirstStationID && item.SecondStationID == twoFollowingStations.SecondStationID) || (item.FirstStationID == twoFollowingStations.SecondStationID && item.SecondStationID == twoFollowingStations.FirstStationID));
            if (f == null)
                throw new StationException("The two following stations do not exist.");
            DataSource.FollowingStations.Remove(f); // remove the old two following stations
            DataSource.FollowingStations.Add(twoFollowingStations.Clone()); // add the updated two following stations
        }
        public TwoFollowingStations getTwoFollowingStations(int firstStationID, int secondStationID)
        {
            TwoFollowingStations twoFollowingStations = DataSource.FollowingStations.Find(item => (item.FirstStationID == firstStationID && item.SecondStationID == secondStationID) || (item.FirstStationID == secondStationID && item.SecondStationID == firstStationID));
            if (twoFollowingStations == null)
                return null;
            return twoFollowingStations.Clone();
        }
        public IEnumerable<TwoFollowingStations> GetFollowingStations()
        {
            return (from item in DataSource.FollowingStations
                    select item.Clone()).OrderBy(item => item.TimeBetweenStations);
        }
        public IEnumerable<TwoFollowingStations> GetFollowingStations(Predicate<TwoFollowingStations> condition)
        {
            return (from item in DataSource.FollowingStations
                    where condition(item)
                    select item.Clone()).OrderBy(item => item.TimeBetweenStations);
        }

        #endregion      

        #region DrivingLines

        public void addDrivingLine(DrivingLine drivingLine)
        {
            try
            {
                getLine(drivingLine.NumberLine); // check if the line exists
            }
            catch (LineException ex)
            {
                throw new LineException(ex.Message);
            }
            if (DataSource.DrivingLines.Exists(item => item.NumberLine == drivingLine.NumberLine && item.Start == drivingLine.Start))
                throw new TripException("The driving line already exists.");
            DataSource.DrivingLines.Add(drivingLine.Clone());
        }
        public void removeDrivingLine(DrivingLine drivingLine)
        {
            XElement rootElem = XMLTools.LoadListFromXMLElement(drivingLinesPath);

            XElement item = (from i in rootElem.Elements()
                             where int.Parse(i.Element("NumberLine").Value) == drivingLine.NumberLine && TimeSpan.Parse(i.Element("Start").Value) == drivingLine.Start
                             select i).FirstOrDefault();

            if (item != null)
            {
                item.Remove();
                XMLTools.SaveListToXMLElement(rootElem, drivingLinesPath);
            }
            else
                throw new TripException("The driving line does not exist.");
        }
        public void updateDrivingLine(DrivingLine drivingLine)
        {
            DrivingLine d = DataSource.DrivingLines.Find(item => item.NumberLine == drivingLine.NumberLine && item.Start == drivingLine.Start);
            if (d == null)
                throw new TripException("The driving line does not exist.");
            DataSource.DrivingLines.Remove(d); // remove the old driving line
            DataSource.DrivingLines.Add(drivingLine.Clone()); // add the updated driving line  
        }
        public DrivingLine getDrivingLine(int numberLine, DateTime start)
        {
            DrivingLine drivingLine = DataSource.DrivingLines.Find(item => item.NumberLine == numberLine && item.Start == new TimeSpan(start.Hour, start.Minute, start.Second));
            if (drivingLine == null)
                return null;
            return drivingLine.Clone();
        }
        public IEnumerable<DrivingLine> GetDrivingLines()
        {
            return from item in DataSource.DrivingLines
                   select item.Clone();
        }
        public IEnumerable<DrivingLine> GetDrivingLines(Predicate<DrivingLine> condition)
        {
            return from item in DataSource.DrivingLines
                   where condition(item)
                   select item.Clone();
        }

        #endregion

        #region ManagingCode

        public string getManagingCode()
        {
            return DataSource.ManagingCode;
        }
        public void updateManagingCode(string code)
        {
            DataSource.ManagingCode = code;
        }

        #endregion
    }
}
