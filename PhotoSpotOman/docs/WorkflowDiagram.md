# PhotoSpotOman - System Workflow Diagrams

## 1. System Architecture Overview

```mermaid
graph TB
    subgraph "Client Layer"
        Browser[Web Browser]
    end

    subgraph "Presentation Layer"
        Views[Razor Views]
        CSS[CSS/Bootstrap]
        JS[JavaScript]
    end

    subgraph "Application Layer"
        HC[HomeController]
        LC[LoginController]
        SC[SpotController]
        CC[CategoryController]
        CmC[CommentController]
        LiC[LikeController]
        FC[FeedbacksController]
    end

    subgraph "Security Layer"
        Auth[Cookie Authentication]
        AuthZ[Authorization Policies]
    end

    subgraph "Data Layer"
        CTX[SpotContext]
        EF[Entity Framework Core]
    end

    subgraph "Database"
        SQL[(SQL Server)]
    end

    subgraph "File Storage"
        FS[/uploads/spots/]
    end

    Browser --> Views
    Views --> CSS
    Views --> JS
    Views --> HC & LC & SC & CC & CmC & LiC & FC
    
    HC & LC & SC & CC & CmC & LiC & FC --> Auth
    Auth --> AuthZ
    AuthZ --> CTX
    CTX --> EF
    EF --> SQL
    SC --> FS
```

---

## 2. Data Model / Entity Relationship Diagram

```mermaid
erDiagram
    USER ||--o{ SPOT : creates
    USER ||--o{ COMMENT : writes
    USER ||--o{ LIKE : gives
    USER ||--o{ SPOT_IMAGE : uploads
    USER ||--o{ REPORT : submits
    
    CATEGORY ||--o{ SPOT : contains
    
    SPOT ||--o{ SPOT_IMAGE : has
    SPOT ||--o{ COMMENT : receives
    SPOT ||--o{ LIKE : receives
    SPOT ||--o| REPORT : reported_as
    
    COMMENT ||--o| REPORT : reported_as

    USER {
        int Id PK
        string Name
        string Email UK
        string Password
        string Role
        bool IsActive
        datetime CreatedAt
        datetime UpdatedAt
    }

    SPOT {
        int Id PK
        string Name
        string Description
        string LocationName
        double Latitude
        double Longitude
        string Region
        string Status
        int AddedBy FK
        int CategoryId FK
    }

    CATEGORY {
        int Id PK
        string Name
        string Description
    }

    SPOT_IMAGE {
        int Id PK
        int SpotId FK
        string ImagePath
        int UploadedBy FK
    }

    COMMENT {
        int Id PK
        int UserId FK
        int SpotId FK
        string Content
        datetime CreatedAt
    }

    LIKE {
        int Id PK
        int UserId FK
        int SpotId FK
    }

    REPORT {
        int Id PK
        int ReportedBy FK
        int SpotId FK
        int CommentId FK
        string Reason
        string Status
        datetime CreatedAt
    }
```

---

## 3. User Authentication Flow

```mermaid
flowchart TB
    Start([User Visits Site]) --> Check{Is Authenticated?}
    
    Check -->|No| LoginPage[Login Page]
    Check -->|Yes| Home[Home Page]
    
    LoginPage --> Action{Action?}
    
    Action -->|Sign Up| SignUp[Fill Registration Form]
    SignUp --> Validate1{Valid Input?}
    Validate1 -->|No| Error1[Show Validation Error]
    Error1 --> SignUp
    Validate1 -->|Yes| CheckEmail{Email Exists?}
    CheckEmail -->|Yes| Error2[Email Already Registered]
    Error2 --> SignUp
    CheckEmail -->|No| HashPwd[Hash Password]
    HashPwd --> CreateUser[Create User with Contributor Role]
    CreateUser --> SaveUser[(Save to Database)]
    SaveUser --> Success1[Success Message]
    Success1 --> LoginPage
    
    Action -->|Sign In| SignIn[Enter Credentials]
    SignIn --> FindUser{User Found?}
    FindUser -->|No| Error3[Account Not Found]
    Error3 --> SignIn
    FindUser -->|Yes| VerifyPwd{Password Valid?}
    VerifyPwd -->|No| Error4[Invalid Password]
    Error4 --> SignIn
    VerifyPwd -->|Yes| CreateClaims[Create Claims Identity]
    CreateClaims --> SetCookie[Set Authentication Cookie]
    SetCookie --> Home
    
    Home --> Logout{Logout?}
    Logout -->|Yes| ClearCookie[Clear Cookie]
    ClearCookie --> LoginPage
    Logout -->|No| Continue[Continue Browsing]
    Continue --> Home
```

---

## 4. Spot Management Workflow

```mermaid
flowchart TB
    subgraph "Contributor Flow"
        Start([Logged In User]) --> Browse[Browse Spots]
        Browse --> ViewDetails[View Spot Details]
        
        Start --> Create[Create New Spot]
        Create --> FillForm[Fill Spot Form]
        FillForm --> Upload[Upload Images]
        Upload --> Submit[Submit]
        Submit --> Pending[Status: Pending]
        Pending --> Wait[Wait for Admin Review]
        
        Start --> MySpots[View My Spots]
        MySpots --> EditSpot[Edit Own Spot]
        EditSpot --> UpdateForm[Update Details/Images]
        UpdateForm --> SaveChanges[Save Changes]
        
        MySpots --> DeleteSpot[Delete Own Spot]
        DeleteSpot --> RemoveImages[Remove Image Files]
        RemoveImages --> DeleteDB[(Delete from Database)]
    end

    subgraph "Public Access"
        Guest([Guest/Anonymous]) --> ViewApproved[View Approved Spots Only]
        ViewApproved --> SpotList[Spot Listing]
        SpotList --> Filter[Filter by Category/Search]
        SpotList --> Details[View Spot Details]
    end

    subgraph "Admin Review"
        Wait --> AdminReview{Admin Decision}
        AdminReview -->|Approve| Approved[Status: Approved]
        AdminReview -->|Reject| Rejected[Status: Rejected]
        Approved --> Public[Visible to Public]
        Rejected --> Hidden[Hidden from Public]
    end
```

---

## 5. Admin Workflow

```mermaid
flowchart TB
    Admin([Admin User]) --> Dashboard[Access All Features]
    
    Dashboard --> SpotMgmt[Spot Management]
    SpotMgmt --> ViewAll[View All Spots - Any Status]
    ViewAll --> FilterStatus[Filter by Status]
    FilterStatus --> Pending[Pending Spots]
    FilterStatus --> Approved[Approved Spots]
    FilterStatus --> Rejected[Rejected Spots]
    
    Pending --> Review{Review Spot}
    Review -->|Approve| SetApproved[Set Status: Approved]
    Review -->|Reject| SetRejected[Set Status: Rejected]
    SetApproved --> Save1[(Save to DB)]
    SetRejected --> Save2[(Save to DB)]
    
    Dashboard --> CatMgmt[Category Management]
    CatMgmt --> ListCats[List Categories]
    ListCats --> CreateCat[Create Category]
    ListCats --> EditCat[Edit Category]
    ListCats --> DeleteCat{Delete Category}
    DeleteCat -->|Has Spots| Error[Cannot Delete]
    DeleteCat -->|No Spots| Remove[(Remove from DB)]
    
    Dashboard --> FeedbackMgmt[Feedback Management]
    FeedbackMgmt --> ViewComments[View All Comments]
    ViewComments --> FilterBySpot[Filter by Spot]
    ViewComments --> DeleteComment[Delete Any Comment]
    DeleteComment --> RemoveComment[(Remove from DB)]
    
    Dashboard --> SpotActions[Any Spot Actions]
    SpotActions --> EditAnySpot[Edit Any Spot]
    SpotActions --> DeleteAnySpot[Delete Any Spot]
    SpotActions --> DeleteAnyImage[Delete Any Image]
```

---

## 6. User Interaction Flow (Likes & Comments)

```mermaid
flowchart TB
    User([Authenticated User]) --> SpotDetails[View Spot Details]
    
    SpotDetails --> LikeSection{Like Section}
    LikeSection --> CheckLiked{Already Liked?}
    CheckLiked -->|Yes| Unlike[Click to Unlike]
    CheckLiked -->|No| Like[Click to Like]
    Unlike --> RemoveLike[(Remove Like)]
    Like --> AddLike[(Add Like)]
    RemoveLike --> UpdateCount1[Update Like Count]
    AddLike --> UpdateCount2[Update Like Count]
    
    SpotDetails --> CommentSection[Comment Section]
    CommentSection --> WriteComment[Write Comment]
    WriteComment --> SubmitComment[Submit Comment]
    SubmitComment --> ValidateContent{Valid Content?}
    ValidateContent -->|No| Error[Show Error]
    ValidateContent -->|Yes| SaveComment[(Save Comment)]
    SaveComment --> DisplayComment[Display New Comment]
    
    CommentSection --> MyComments[View Own Comments]
    MyComments --> DeleteOwn[Delete Own Comment]
    DeleteOwn --> RemoveComment[(Remove Comment)]
    
    subgraph "Admin Only"
        CommentSection --> AllComments[View All Comments]
        AllComments --> DeleteAny[Delete Any Comment]
        DeleteAny --> AdminRemove[(Remove Comment)]
    end
```

---

## 7. Authorization & Access Control

```mermaid
flowchart LR
    subgraph "Roles"
        Admin[Admin]
        Contributor[Contributor]
        Guest[Anonymous/Guest]
    end

    subgraph "Public Pages"
        P1[Home Page]
        P2[View Approved Spots]
        P3[Spot Details]
        P4[Login/Register]
    end

    subgraph "Authenticated Pages"
        A1[Create Spot]
        A2[My Spots]
        A3[Edit Own Spot]
        A4[Delete Own Spot]
        A5[Like/Unlike Spots]
        A6[Add Comments]
        A7[Delete Own Comments]
    end

    subgraph "Admin Only Pages"
        AD1[Category Management]
        AD2[Approve/Reject Spots]
        AD3[Feedback Management]
        AD4[Edit Any Spot]
        AD5[Delete Any Spot]
        AD6[Delete Any Comment]
        AD7[View All Status Spots]
    end

    Guest --> P1 & P2 & P3 & P4
    
    Contributor --> P1 & P2 & P3
    Contributor --> A1 & A2 & A3 & A4 & A5 & A6 & A7
    
    Admin --> P1 & P2 & P3
    Admin --> A1 & A2 & A3 & A4 & A5 & A6 & A7
    Admin --> AD1 & AD2 & AD3 & AD4 & AD5 & AD6 & AD7
```

---

## 8. Complete Application Flow

```mermaid
flowchart TB
    Start([Start]) --> Visit[User Visits PhotoSpotOman]
    
    Visit --> IsAuth{Authenticated?}
    
    IsAuth -->|No| GuestActions[Guest Actions]
    GuestActions --> G1[View Home Page]
    GuestActions --> G2[Browse Approved Spots]
    GuestActions --> G3[Search/Filter Spots]
    GuestActions --> G4[View Spot Details]
    GuestActions --> G5[Login/Register]
    
    G5 --> Auth[Authentication]
    Auth --> Register[Register as Contributor]
    Auth --> Login[Login]
    Register --> Login
    Login --> Authenticated
    
    IsAuth -->|Yes| Authenticated[Authenticated User]
    
    Authenticated --> RoleCheck{User Role?}
    
    RoleCheck -->|Contributor| ContribActions[Contributor Actions]
    ContribActions --> C1[All Guest Actions]
    ContribActions --> C2[Create New Spots]
    ContribActions --> C3[Manage Own Spots]
    ContribActions --> C4[Like/Unlike Spots]
    ContribActions --> C5[Comment on Spots]
    ContribActions --> C6[View My Spots]
    
    RoleCheck -->|Admin| AdminActions[Admin Actions]
    AdminActions --> A0[All Contributor Actions]
    AdminActions --> A1[Manage Categories]
    AdminActions --> A2[Approve/Reject Spots]
    AdminActions --> A3[Manage All Spots]
    AdminActions --> A4[Manage All Comments]
    AdminActions --> A5[View All Spot Statuses]
    
    subgraph "Spot Lifecycle"
        C2 --> Created[Spot Created]
        Created --> PendingStatus[Status: Pending]
        PendingStatus --> A2
        A2 -->|Approve| ApprovedStatus[Status: Approved]
        A2 -->|Reject| RejectedStatus[Status: Rejected]
        ApprovedStatus --> PublicView[Visible to All Users]
        RejectedStatus --> OwnerOnly[Visible to Owner Only]
    end
    
    ContribActions & AdminActions --> Logout[Logout]
    Logout --> GuestActions
```

---

## 9. Image Upload & Management Flow

```mermaid
flowchart TB
    User([User]) --> CreateEdit[Create/Edit Spot]
    
    CreateEdit --> SelectImages[Select Image Files]
    SelectImages --> Validate{Valid Images?}
    
    Validate -->|No| Error[Show Error]
    Error --> SelectImages
    
    Validate -->|Yes| Process[Process Each Image]
    
    Process --> GenerateName[Generate Unique Filename]
    GenerateName --> SaveFile[Save to /uploads/spots/]
    SaveFile --> CreateRecord[Create SpotImage Record]
    CreateRecord --> LinkSpot[Link to Spot]
    LinkSpot --> SaveDB[(Save to Database)]
    
    subgraph "Image Deletion"
        User --> EditSpot[Edit Spot]
        EditSpot --> ViewImages[View Current Images]
        ViewImages --> DeleteImage[Delete Image]
        DeleteImage --> CheckOwner{Is Owner or Admin?}
        CheckOwner -->|No| Deny[Access Denied]
        CheckOwner -->|Yes| DeleteFile[Delete File from Server]
        DeleteFile --> DeleteRecord[(Delete from Database)]
    end
    
    subgraph "Spot Deletion"
        User --> DeleteSpot[Delete Spot]
        DeleteSpot --> GetImages[Get All Spot Images]
        GetImages --> LoopDelete[Loop: Delete Each File]
        LoopDelete --> DeleteSpotRecord[(Delete Spot + Images from DB)]
    end
```

---

## 10. API Request Flow

```mermaid
sequenceDiagram
    participant B as Browser
    participant M as Middleware
    participant A as Authentication
    participant Az as Authorization
    participant C as Controller
    participant Ctx as SpotContext
    participant DB as SQL Server
    participant FS as File System

    B->>M: HTTP Request
    M->>M: Use Static Files
    M->>M: Use Routing
    M->>A: Authentication Check
    
    alt Unauthenticated
        A->>B: Redirect to Forbidden (401)
    else Authenticated
        A->>Az: Check Authorization Policy
        
        alt Unauthorized
            Az->>B: Redirect to Forbidden (403)
        else Authorized
            Az->>C: Route to Controller Action
            C->>Ctx: Query/Command
            Ctx->>DB: Execute SQL
            DB->>Ctx: Return Data
            
            opt File Operations
                C->>FS: Read/Write Files
                FS->>C: File Result
            end
            
            Ctx->>C: Return Entities
            C->>B: Return View/JSON
        end
    end
```

---

## Legend

| Symbol | Meaning |
|--------|---------|
| `([text])` | Start/End Point |
| `[text]` | Process/Action |
| `{text}` | Decision |
| `[(text)]` | Database Operation |
| `[/text/]` | File Storage |
| `-->` | Flow Direction |
| `-->\|label\|` | Conditional Flow |

---

## Controllers Summary

| Controller | Access Level | Purpose |
|------------|--------------|---------|
| `HomeController` | Public | Home page, popular spots display |
| `LoginController` | Public | User authentication (SignIn/SignUp) |
| `SpotController` | Mixed | Spot CRUD, approval (Admin), filtering |
| `CategoryController` | Admin Only | Category CRUD management |
| `CommentController` | Authenticated | Comment CRUD on spots |
| `LikeController` | Authenticated | Like/unlike toggle on spots |
| `FeedbacksController` | Admin Only | View/manage all comments |
| `LogOutController` | Authenticated | User logout |
| `PageController` | Public | Error pages (Forbidden) |

---

## Authorization Policies

| Policy | Roles | Usage |
|--------|-------|-------|
| `AdminOnly` | Admin | Category management, spot approval, feedback management |
| `ContributorOnly` | Contributor | (Reserved for future use) |
| `AnyRole` | Admin, Contributor | Spot creation, commenting, liking |






