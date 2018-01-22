USE organizer
GO

CREATE PROCEDURE GetAllEvents
AS
SELECT E.Id, E.Name, E.Priority, E.Note, E.Done, B.DateOfBirth, H.Date, J.Start, J.Deadline,
M.Start AS MeetingStart, M.Ending, M.Location, R.AlarmTime
FROM Event as E
LEFT JOIN Birthday AS B ON E.Id = B.Id
LEFT JOIN Holiday AS H ON E.Id = H.Id
LEFT JOIN Job AS J ON E.Id = J.Id
LEFT JOIN Meeting AS M ON E.Id = M.Id
LEFT JOIN Reminder as R ON E.Id =R.Id
GO