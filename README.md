# 🏠 PropEase – Smart Property Management Portal

PropEase (also known as **PropSmart**) is a modern, full-featured real estate web application built using **ASP.NET Core MVC**, **Entity Framework Core**, and **SQL Server**.  
It allows users to **browse, search, and manage properties**, **contact owners**, **save favorites**, and enables **admins and owners** to manage property listings efficiently.

---

## 🚀 Features

### 👤 User Features
- **Property Browsing** – View all available properties with details and images.
- **Advanced Search & Filters** – Search by location, price range, type, and more.
- **Property Details Page** – Displays complete property information, including owner details.
- **Wishlist** – Save or remove favorite properties with a heart toggle button ❤️.
- **Contact Owner** – Send a message directly to the property owner using a form.
- **Responsive Design** – Optimized for desktop and mobile.

### 🧑‍💼 Owner Features
- Add, edit, and delete property listings.
- Manage all owned properties from a single dashboard.
- View and respond to contact messages from interested users.

### 🛡️ Admin Features
- Full control over users, roles, and property data.
- Access all contact messages from the admin dashboard.
- Assign or revoke roles (Admin / Owner / User).
- Manage reported or inactive listings.

### 🔒 Authentication & Authorization
- ASP.NET Identity-based authentication.
- Role-based access control for Admin, Owner, and Regular User.
- Secure login, registration, and session management.

---

## 🏗️ Tech Stack

| Layer | Technology |
|-------|-------------|
| **Frontend** | HTML5, CSS3, Bootstrap 5, Razor Views |
| **Backend** | ASP.NET Core MVC (.NET 9) |
| **Database** | SQL Server (via Entity Framework Core ORM) |
| **Authentication** | ASP.NET Identity |
| **Version Control** | Git + GitHub |
| **IDE** | Visual Studio 2022 / VS Code |

---

## ⚙️ Setup Instructions

### 1. Clone the repository
bash
git clone https://github.com/<your-username>/PropEase.git
cd PropEase

### 2. Configure the database
Edit the connection string in appsettings.json:
json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PropEaseDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}

### 3. Apply migrations
Run the following commands in the Package Manager Console:
bash
Add-Migration InitialCreate
Update-Database

### 4. Run the application
bash
dotnet run
Then open your browser and navigate to:
arduino
https://localhost:5001


### 👥 User Roles Overview
Role	Permissions
Admin	Full control over users, properties, and contact messages
Owner	Can add/edit/delete own properties and view contact inquiries
User	Can browse, search, favorite, and contact property owners

### 💬 Contact Message Flow
A user clicks “Contact Owner” on a property.

The contact form (ContactOwner.cshtml) is submitted to the ContactController.

A new ContactMessage is saved in the database.

A success message appears, and admins can view it on the dashboard.

### ❤️ Wishlist Functionality
Clicking the 💖 button on a property toggles it as favorite.

Favorites are stored per user in the Favorites table.

On the Wishlist page, users can view and remove saved properties without redirection after removal.

### 🧩 Future Enhancements
Add multiple image upload functionality for properties.

Implement email notifications for contact messages.

Integrate Google Maps for property locations.

Add advanced filters (e.g., amenities, items).

Implement REST APIs for mobile app integration.