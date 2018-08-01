# Ideas
- Publish Calendars
- Email Participants, Users
- Participants watch Calendar
  - Calendars with Participant Lists
- Time sensitive Openings
- Time sensitive Calendars
- Calendars have Attributes that can be assigned to Users
- Openings have Attributes to define who can Participate
- Activities have Attributes to define who can Participate
- Events have Attributes to define who can Participate
- Not all Openings require Participants
- Make a Calendar visible to Users, Accounts, etc

# Structure
- Calendar
  - belongs to Account
  - has Users
  - has Attributes (key|value)
    - belongs to Calendar
    - has Rules
  - has Tags
  - has Events
    - belongs to Calendar
    - has Attributes
    - has Tags
    - has Activities
      - belongs to Event
      - has Openings
        - has Attributes
        - has Participants
          - is User
- User
  - has Roles
  - has Accounts
  - has Calendars
  - has Attributes (key|value)
- Account
  - has Users
  - has Calendars
- Schedule
  - has Calendar with date range

# Calendar
A calendar provides a way to manage events.

| Property      | Type      | Size  | Required  | Index     |
|---------------|-----------|-------|-----------|-----------|
| Id            | INT       |       | Yes       | PRIMARY   |
| Key           | GUID      |       | Yes       | UNIQUE    |
| Name          | NVARCHAR  | 250   | Yes       | UNIQUE    |
| Description   | NVARCHAR  | 2000  | No        |           |
| State         | INT       |       | Yes       | Yes       |

# Event
An event provides a way to manage a specific occasion on a calendar.

| Property      | Type      | Size  | Required  | Index     |
|---------------|-----------|-------|-----------|-----------|
| Id            | INT       |       | Yes       | PRIMARY   |
| Key           | GUID      |       | Yes       | UNIQUE    |
| CalendarId    | INT       |       | Yes       |
| Name          | NVARCHAR  | 250   | Yes       | UNIQUE    |
| Description   | NVARCHAR  | 2000  | No        |           |
| StartDate     | DATETIME2 |       | Yes       | Yes       |
| EndDate       | DATETIME2 |       | Yes       | Yes       |
| State         | INT       |       | Yes       | Yes       |
| Type          | INT       |       | Yes       | Yes       |

# Activity
| Property      | Type      | Size  | Required  | Index     |
|---------------|-----------|-------|-----------|-----------|
| Id            | INT       |       | Yes       | PRIMARY   |
| Key           | GUID      |       | Yes       | UNIQUE    |
| EventId       | INT       |       | Yes       |
| Name          | NVARCHAR  | 250   | Yes       | UNIQUE    |
| Description   | NVARCHAR  | 2000  | No        |           |
| State         | INT       |       | Yes       | Yes       |
| Type          | INT       |       | Yes       | Yes       |
| StartDate     | DATETIME2 |       | Yes       | Yes       |
| EndDate       | DATETIME2 |       | Yes       | Yes       |

# Participant
| Property      | Type      | Size  | Required  | Index     |
|---------------|-----------|-------|-----------|-----------|
| Id            | INT       |       | Yes       | PRIMARY   |
