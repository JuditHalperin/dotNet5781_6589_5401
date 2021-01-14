﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BLAPI
{
    public interface IBL
    {
        #region Users

        void addUser(User user);
        void removeUser(User user);
        void updateUser(User user);
        User getUser(string username);
        IEnumerable<User> GetUsers();
        IEnumerable<User> GetUsers(Predicate<User> condition);

        #endregion

        #region Buses
        void addBus(Bus bus);
        void removeBus(Bus bus);
        void updateBus(Bus bus);
        Bus getBus(string licensePlate);
        IEnumerable<Bus> GetBuses();
        IEnumerable<Bus> GetBuses(Predicate<Bus> condition);
        void fuelBus(Bus bus);
        void serviceBus(Bus bus);

        #endregion

        #region Lines

        void addLine(Line line);
        void removeLine(Line line);
        void updateLine(Line line);
        Line getLine(int serial);
        IEnumerable<Line> GetLines();
        IEnumerable<Line> GetLines(Predicate<Line> condition);
        bool canChangeLine(Line line);
        int countLines();

        #endregion

        #region Stations

        void addStation(Station station);
        void removeStation(Station station);
        void updateStation(Station station, int distanceFromPreviousLocation);
        Station getStation(int id);
        IEnumerable<Station> GetStations();
        IEnumerable<Station> GetStations(Predicate<Station> condition);
        bool canChangeStation(Station station);
        int countStations();

        #endregion

        #region LineStations

        IEnumerable<LineStation> convertToLineStationsList(IEnumerable<Station> path);
        IEnumerable<Station> convertToStationsList(IEnumerable<LineStation> path);
        void addLineStation(LineStation lineStation);
        void removeLineStation(LineStation lineStation);
        void updateLineStation(LineStation lineStation);
        LineStation getLineStation(int numberLine, int id);
        IEnumerable<LineStation> GetLineStations();
        IEnumerable<LineStation> GetLineStations(Predicate<LineStation> condition);

        #endregion

        #region TwoFollowingStations
        void addTwoFollowingStations(int firstID, int secondID, int length);
        bool getTwoFollowingStations(int firstID, int secondID);

        #endregion

        #region DrivingBuses

        IEnumerable<DrivingBus> getPassengerTrips(int source, int target);
        TimeSpan timeTillArrivalToSource(DrivingBus trip, int source);
        TimeSpan durationTripBetweenStations(int serial, int source, int target);
        //IEnumerable<DrivingBus> GetTripsOfLine_Present(int serial);
        //IEnumerable<DrivingBus> GetTripsOfLine_Future(int serial);
        //IEnumerable<DrivingBus> GetTrips();

        #endregion

        #region DrivingLines

        void addDrivingLine(DrivingLine drivingLine);
        void removeDrivingLine(DrivingLine drivingLine);
        void updateDrivingLine(DrivingLine drivingLine);
        DrivingLine getDrivingLine(int numberLine, TimeSpan start);
        IEnumerable<DrivingLine> GetDrivingLines();
        IEnumerable<DrivingLine> GetDrivingLines(Predicate<DrivingLine> condition);
        IEnumerable<TimeSpan> getTripsStart(int numberLine);

        #endregion

        #region ManagingCode

        string getManagingCode();
        void updateManagingCode(string code);
        
        #endregion
    }
}
