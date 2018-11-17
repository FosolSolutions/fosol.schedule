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
- Provide a way for participants to apply for an opening
- Provide a way to invite non-users to participate
- Allow a user to participate

# Structure
- Calendar "an endless calendar to contain all of your events"
  - belongs to Account
  - has Participants
  - has Criteria
    - belongs to Calendar
    - has Rules
  - has Tags
  - has Events
    - belongs to Calendar
    - has Criterias
    - has Tags
    - has Activities
      - belongs to Event
      - has Criterias
      - has Openings
        - has Criterias
        - has Participants
          - may be a User
          - has Attributes
- Criteria "a way to identify and filter qualifying criteria"
  - has Attributes
  - has Rules
- Attribute "an attribute which identifies a qualification of a user"
  - has key|value pair
- User "a user who has access to the application"
  - has Roles
  - has Accounts
  - has Calendars
  - has Attributes
  - is many Participants
    - each Calendar will have it's own participants
- Participant "a person particating in an opening"
  - may be a User
  - has Attributes
    - are Calendar specific
- Account "an account which is owned by a user"
  - has Users
  - has Calendars
- Schedule "a way to filter events from calendars"
  - has Events
  - StartOn
  - EndOn
  - belongs to Account
- Opening
  - has Questions
    - has QuestionOption
      - Sequence
      - belongs to a Question
      - Value
    - Caption (i.e. Title)
    - IsRequired
    - Text (i.e. What is the title of your Bible talk?)
    - AnswerType [string,int,float,decimal,bool,checkbox,list,select]
    - MaxLength
    - Sequence
    - belong to an Account
    - has OpeningParticipantAnswer
      - OpeningParticipant
      - Question
      - QuestionOption
      - Text

---
# Calendar
A calendar provides a way to manage events.

| Property      | Type      | Size  | Required  | Index     |
|---------------|-----------|-------|-----------|-----------|
| Id            | INT       |       | Yes       | PRIMARY   |
| Key           | GUID      |       | Yes       | UNIQUE    |
| Name          | NVARCHAR  | 250   | Yes       | UNIQUE    |
| Description   | NVARCHAR  | 2000  | No        |           |
| State         | INT       |       | Yes       | Yes       |

# CalendarEvent
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

# EventActivity
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
