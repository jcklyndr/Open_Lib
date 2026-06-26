# 📖 OPEN_LIB SYSTEM

A simple, modern, and vibrant web application built to serve as a centralized hub for free reading material. [cite_start]The core mission of **OPEN_LIB** is to bridge the gap between avid readers and knowledge by allowing users to access, discover, and request or download books (but mostly request) completely free of charge—without paying for anything[cite: 446].

**Developer's Note:** This is my very first project developed using the **ASP.NET Core** framework!.It is currently an active work in progress as I continuously update and optimize the system to make it more dynamic and incorporate additional features

---

## Features Breakdown

* **Modern Authentication:** Clean, sleek user Login and Signup interfaces styled with a high-contrast Teal Glow palette and secure client-side validation utilities
**Interactive Password Toggle:** Dynamic visual masks implemented on all password inputs across login and registration forms to ensure an effortless typing experience
**Categorized Catalog:** Books sorted clearly into distinct categories, displayed using responsive Bootstrap 5 cards equipped with crisp cover image renders
* **Seamless Book Requesting:** Detailed book description layout with an integrated sticky request form that follows desktop scroll behaviors flawlessly
**Live Request Tracking:** A dedicated "My Request" dashboard where users can see their book request list and view real-time status updates changing dynamically based on database parameters
* **Smart Empty State Fallbacks:** No blank screens—if a user hasn't made any requests yet, an aesthetic placeholder guides them back to the catalog

---

## Technology Stack
* [cite_start]**Backend Framework:** ASP.NET Core MVC (C# / Razor Engine) 
* **Frontend Ecosystem:** Bootstrap 5 (Responsive Utilities), Font Awesome 6 (Vector Icons Library), AOS (Animate on Scroll Framework) 
* **Dependency Engine:** Node.js / npm 
* **Database Management:** Microsoft SQL Server 

---

## 🚀 Getting Started & Configuration

### 1. Database Connection String
[cite_start]To link this system with your local Microsoft SQL Server environment, configure the connection block within your root `appsettings.json` file Insert your specific server instance handle into the template below:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=;Database=OpenLibDb;Trusted_Connection=True;TrustServerCertificate=true;"
  },
  "AllowedHosts": "*"
}
```

### 2. Install Frontend Dependencies

```bash
# Initialize node modules map
npm init -y

# Download active UI bundles
npm install bootstrap aos @fortawesome/fontawesome-free
```

> 💡 **Note:** Ensure all downloaded package distributions are mapped correctly inside your static workspace paths under `wwwroot/lib/` before launching your Visual Studio solution debugger.

---

## 📈 Roadmap & Upcoming Features
As this project remains actively on progress, the following milestone upgrades are slated for future implementation to make the system even more dynamic:

- [ ] Direct file download functionality for open-source documents.
- [ ] Real-time email layout notification hooks when a book request status shifts.
- [ ] Advanced administrative dashboards for tracking catalog inventory metrics.
- [ ] Soft search filtration systems on the main book category indices.

---

## 📸 System Preview
Here is a glimpse of how the Open_Lib System looks with its modern design implementation:

![Open_Lib Preview](wwwroot/images/Open_Lib.png)
