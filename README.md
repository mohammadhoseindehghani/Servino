# 🏠 Servino – Home Services Web Application

<p align="center">
  <img src="https://img.shields.io/github/repo-size/mohammadhoseindehghani/Servino" />
  <img src="https://img.shields.io/github/license/mohammadhoseindehghani/Servino" />
  <img src="https://img.shields.io/badge/ASP.NET%20Core-Onion%20Architecture-blueviolet" />
</p>

---

## 🌟 Project Overview

**Servino** is a modern **Home Services Management Platform** that connects  
**Customers**, **Service Experts**, and **Administrators** in a unified system.

The project is built using **ASP.NET Core** and follows **Onion Architecture** to ensure:

- Clean separation of concerns  
- High maintainability  
- Scalability for real-world applications  

---

## 👥 User Roles & Capabilities

### 👤 Customers
- Browse available home services 🛠️
- Submit service requests 📝
- Review and select expert proposals ✅
- Confirm orders and make payments 💳
- Rate services and leave feedback ⭐

### 🧑‍🔧 Service Experts
- View requests matching their skills 👀
- Submit proposals ✍️
- Track assigned jobs 📋
- Complete services and receive payments 💰

### 🛡️ Administrators
- Manage users, services, and categories 🗂️
- Monitor requests and proposals 📌
- Moderate comments and ratings 📝
- Access system reports 📊

---

## 🏗️ Architecture

### Layer Responsibilities

- **Domain.Core**
  - Entities
  - Value Objects
  - DTOs
  - Contracts (Interfaces)

- **Domain.Service**
  - Business logic implementations

- **Domain.AppService**
  - Application-level orchestration
  - Use-case coordination

- **Infrastructure**
  - EF Core & Dapper
  - DbContext & configurations
  - Identity, OTP, and external providers

- **Presentation**
  - ASP.NET MVC / Razor Pages
  - Bootstrap-based UI

> 🔹 Dependencies always flow inward.  
> Outer layers depend on inner layers, never the opposite.

---

## 🚀 Phase 1 – Foundation & Core Setup

✅ System analysis and domain modeling  
✅ Onion Architecture project structure  
✅ Core entities and relationships  
✅ EF Core DbContext & Fluent API configurations  
✅ Seed data for categories and services  
✅ CRUD operations for Categories & Services  
✅ Stable build and ready-to-extend foundation  

> **Phase 1** established a solid architectural base for future development.

---

## 🚀 Phase 2 – Core Features & Management Panels

In this phase, Servino evolved into a **fully functional platform** without changing the core architecture.

---

### 🔹 Logging & Monitoring

- Logging implemented across **all layers**
- **Serilog** used as the logging provider
- Centralized log storage with **Seq**
- Logged events include:
  - System exceptions and errors
  - Critical user actions
  - Order lifecycle and status changes

---

### 🔹 Authentication & Authorization

- Full **Register & Login** implementation using **Microsoft Identity**
- Role-based access control:
  - Customer
  - Expert
  - Admin
- Authorization policies applied throughout the system

---

### 🔹 OTP-Based Authentication

- Implemented **OTP (One-Time Password)** authentication
- Supported login methods:
  - Email + OTP
  - Mobile number + OTP
- Secure handling of:
  - OTP generation
  - Expiration and validation
  - Retry and abuse protection
- OTP logic implemented in **Infrastructure** and consumed via **Application Services**

---

### 🔹 User Profile Management

- Display customer and expert profile information
- Edit and update profile details
- Profile logic isolated in Application Services

---

### 🔹 Admin Panel

A complete **Admin Dashboard** with:

- User management (**CRUD**)
- Service management (**CRUD**)
- Category management (**CRUD**)
- Viewing expert proposals per request
- Comment & rating moderation (approve / reject)

---

### 🔹 Soft Delete Strategy

- All delete operations use **Soft Delete**
- No physical deletion from the database
- Common fields:
  - `IsDeleted`
  - `DeletedAt`
- Global Query Filters applied via EF Core

> **Phase 2** significantly improves security, observability, and system administration while preserving Onion Architecture principles.

---

## ⚙️ Key Technical Features

- ASP.NET Core
- EF Core & Dapper
- Fluent API entity configurations
- Microsoft Identity
- OTP-based authentication
- Serilog + Seq centralized logging
- Persian (Jalali) date support 📅
- Pagination & filtering
- Secure file storage (disk-based) 🖼️
- Soft Delete applied across all entities
- SOLID principles & clean code practices

---

## 🌐 Getting Started

### 1️⃣ Clone the repository
```bash
git clone https://github.com/mohammadhoseindehghani/Servino.git
```


