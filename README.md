# 🧠 NeuroTab — Thinking Workspace (Backend)

This is the **backend API** for [NeuroTab](https://github.com/LucaOr12/NeuroTab), a thinking workspace where thoughts live as interconnected tabs and AI assists your idea process.

Built with **ASP.NET Core Web API** and **PostgreSQL**, with integrated AI features powered by OpenRouter.

---

## 🛠️ Tech Stack

- **ASP.NET Core 8.0** (Web API)
- **PostgreSQL** (via Supabase)
- **Entity Framework Core**
- **OpenRouter API**
- **Google OAuth2** 
- **Dockerized** (optional)
- **Deployed on**: Render Hosting

---

## 📁 Project Structure
neurotab-api/<br>
├── Controllers/ # REST endpoints (Tabs, Auth, AI)<br>
├── Models/ # Entity models<br>
├── DTOs/ # Data Transfer Objects<br>
├── Services/ # Business logic & AI calls<br>
├── Data/ # DbContext + migrations<br>
├── Middleware/ # JWT, error handling, etc.<br>
└── Program.cs # App startup<br>

---

## 🧠 About the Project

NeuroTab is a Full Stack productivity tool built to:
- Explore and demonstrate real-world AI integration
- Showcase clean API architecture
- Support secure, multi-user functionality
- Emphasize modularity, scalability, and maintainability