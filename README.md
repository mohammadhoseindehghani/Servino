# 🏠 Servino - Home Services Web Application

[![GitHub repo size](https://img.shields.io/github/repo-size/mohammadhoseindehghani/Servino)](https://github.com/mohammadhoseindehghani/Servino)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/mohammadhoseindehghani/Servino/blob/main/LICENSE)

Servino is a modern web platform for managing **home services**, connecting **customers**, **service experts**, and **administrators** seamlessly. This project is developed in **ASP.NET Core** following **Onion Architecture** for clean, maintainable, and scalable code.

---

## 🌟 Project Overview

- **Customers** can:
  - Browse available services 🛠️
  - Submit service requests 📝
  - Compare and select expert proposals ✅
  - Confirm orders and make payments 💳
  - Leave ratings and feedback ⭐

- **Service Experts** can:
  - View requests matching their skills 👀
  - Submit proposals ✍️
  - Track order details 📋
  - Perform services and receive payments 💰

- **Administrators** can:
  - Manage services and categories 🗂️
  - Review requests and assign statuses 📌
  - Manage feedback and ratings 📝
  - Generate reports 📊

---

## 🏗️ Architecture

Servino follows a **layered Onion Architecture**:

- **Domain.Core** – Entities, Value Objects, DTOs, Contracts (Interfaces)  
- **Domain.Service** – Service Implementations (business logic)  
- **Domain.AppService** – Application services orchestrating multiple domain services  
- **Infrastructure** – Data access (EF Core, Dapper), DbContext, Configurations, External Providers  
- **Presentation** – MVC / Razor Pages UI, styled with **Bootstrap**  

> 🔹 Dependencies flow inward. Presentation → AppService → Service → Core → Entities

---

## 🚀 Phase 1 Highlights

✅ Initial analysis & design completed  
✅ Core entities and properties created  
✅ Project layers structured according to Onion Architecture  
✅ DbContext created & configured  
✅ Entity relationships defined  
✅ Seed data added for initial setup  
✅ CRU operations implemented for categories & services  
✅ Project builds successfully  

> Phase 1 sets the foundation for the entire project, ensuring the system is stable and extendable for future phases.

---

## ⚙️ Key Features

- EF Core & Dapper for optimized data access  
- Fluent API for entity configurations  
- Role-based authentication using Microsoft Identity 🔒  
- Persian (Jalali) date handling across the system 📅  
- Pagination & filtering for large datasets 📄  
- Secure file management: images stored on disk only 🖼️  
- Consistent naming conventions and coding standards 💻  

---

## 🌐 Getting Started

1. Clone the repository:  
   ```bash
   git clone https://github.com/mohammadhoseindehghani/Servino.git
