# APTT — Academic Term Planning App

A cross-platform mobile application built with **.NET MAUI** for tracking academic terms, courses, and assessments. Data is stored locally on device using SQLite. No backend or internet connection is required.

---

## Features

- **Terms** — Create and manage academic terms with a name, start date, and end date.
- **Courses** — Add courses to a term with instructor contact info (name, phone, email), status, notes, and date range.
- **Assessments** — Add up to two assessments per course (one *Objective*, one *Performance*), each with a name, type, and date range.
- **Local Notifications** — Opt-in push notifications for course and assessment start/end dates, delivered at 6:00 AM on the scheduled day.
- **Full CRUD** — Create, view, edit, and delete terms, courses, and assessments.

---

## Tech Stack

| Layer        | Technology                        |
|--------------|-----------------------------------|
| Framework    | .NET 9.0 MAUI                     |
| UI           | XAML                              |
| Database     | SQLite via `sqlite-net-pcl`       |
| Notifications| `Plugin.LocalNotification` 12.x   |
| Platforms    | Android, iOS, macOS, Windows      |

---

## Running Locally

> **Note:** This app is not hosted and is intended to be deployed directly to a device or emulator/simulator.

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- .NET MAUI workload:
  ```
  dotnet workload install maui
  ```
- **Android:** Android SDK (API 21+), an emulator or physical device
- **iOS / macOS:** macOS with Xcode installed (iOS 15.0+ / macOS 12.0+)
- **Windows:** Windows 10 (build 17763+)

### Build & Run

Clone the repository and open the solution, then use one of the following:

**Visual Studio 2022 (Windows / macOS)**
1. Open `mobile-app.sln`
2. Select a target framework and device from the toolbar
3. Press **F5** to build and run

**CLI**
```bash
# Android emulator
dotnet build -f net9.0-android
dotnet run -f net9.0-android

# iOS simulator (macOS only)
dotnet build -f net9.0-ios
dotnet run -f net9.0-ios

# macOS
dotnet build -f net9.0-maccatalyst
dotnet run -f net9.0-maccatalyst

# Windows
dotnet build -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-windows10.0.19041.0
```

### Database

The SQLite database (`terms.db3`) is created automatically on first launch inside the app's local data directory (`FileSystem.AppDataDirectory`). No setup or migrations are needed.

---

## Project Structure

```
├── Models/                 # Term, Course, Assessment data models
├── Database/               # DatabaseService — SQLite CRUD operations
├── View/                   # All pages (Add/Edit/Detail for Terms, Courses, Assessments)
├── Resources/              # Fonts, styles, icons, splash screen
├── Platforms/              # Platform-specific entry points (Android, iOS, macOS, Windows)
├── MainPage.xaml(.cs)      # Terms list — home screen
├── AppShell.xaml(.cs)      # Shell navigation setup
└── MauiProgram.cs          # App startup and DI registration
```