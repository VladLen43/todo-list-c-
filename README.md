# TODO List

Простое приложение для управления задачами на WPF и ASP.NET Core.

## Что это

Приложение состоит из двух частей:
- **API** (ASP.NET Core) - серверная часть с REST API
- **WPF клиент** - десктопное приложение для Windows
- **SQLite** - база данных

## Основные функции

- Регистрация и вход (JWT токены)
- Создание, редактирование и удаление задач
- Приоритеты и статусы задач
- Категории с цветами
- Теги для задач
- Поиск и фильтрация
- Статистика по задачам

## Технологии

**Backend:**
- .NET 6.0
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- JWT аутентификация

**Frontend:**
- WPF (.NET 6.0)
- MVVM паттерн
- Material Design

## Запуск

### Через .NET CLI

1. Запустите API:
```bash
cd TodoListApp.API
dotnet run
```
API будет на `http://localhost:5000`

2. В другом терминале запустите WPF:
```bash
cd TodoListApp.WPF
dotnet run
```

### Через Visual Studio

Откройте `TodoListApp.sln`, настройте запуск обоих проектов (API и WPF) и нажмите F5.

## Использование

1. Запустите API сервер
2. Запустите WPF приложение
3. Зарегистрируйтесь или войдите
4. Создавайте задачи, назначайте категории, устанавливайте приоритеты

## API Endpoints

**Аутентификация:**
- `POST /api/auth/register` - регистрация
- `POST /api/auth/login` - вход

**Задачи:**
- `GET /api/tasks` - список задач
- `POST /api/tasks` - создать задачу
- `PUT /api/tasks/{id}` - обновить задачу
- `DELETE /api/tasks/{id}` - удалить задачу
- `GET /api/tasks/statistics` - статистика

**Категории:**
- `GET /api/categories` - список категорий
- `POST /api/categories` - создать категорию

Полная документация API доступна в Swagger: `http://localhost:5000/swagger`

## Структура проекта

```
TodoListApp/
├── TodoListApp.Core/     # Общие модели и DTOs
├── TodoListApp.API/      # Web API
└── TodoListApp.WPF/      # WPF клиент
```

## Требования

- Windows 10/11
- .NET 6.0 SDK
- Visual Studio 2022 (опционально)

## Известные ограничения

- API работает только на localhost
- Нет синхронизации в реальном времени
- Только русский язык

## Безопасность

- Пароли хешируются через BCrypt
- JWT токены для аутентификации
- Пользователи видят только свои данные

---
Учебный проект, 2026
