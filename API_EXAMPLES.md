# 📡 Примеры использования API

## Базовый URL
```
http://localhost:5000/api
```

## 🔐 Аутентификация

### Регистрация нового пользователя

**Запрос:**
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "SecurePass123",
  "firstName": "Джон",
  "lastName": "Доу"
}
```

**Ответ (200 OK):**
```json
{
  "userId": 1,
  "username": "john_doe",
  "email": "john@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Вход в систему

**Запрос:**
```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "john_doe",
  "password": "SecurePass123"
}
```

**Ответ (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Ошибка (401 Unauthorized):**
```json
{
  "message": "Неверное имя пользователя или пароль"
}
```

## 📋 Задачи

> **Примечание:** Все запросы к задачам требуют авторизации. Добавьте заголовок:
> ```
> Authorization: Bearer {ваш_токен}
> ```

### Получить все задачи

**Запрос:**
```http
GET /api/tasks
Authorization: Bearer {token}
```

**Ответ (200 OK):**
```json
[
  {
    "taskId": 1,
    "title": "Изучить ASP.NET Core",
    "description": "Пройти официальную документацию",
    "priority": 2,
    "status": 1,
    "categoryId": 1,
    "categoryName": "Работа",
    "createdAt": "2026-02-03T10:00:00Z",
    "dueDate": "2026-02-15T00:00:00Z",
    "completedAt": null,
    "tags": ["обучение", "программирование"]
  },
  {
    "taskId": 2,
    "title": "Купить продукты",
    "description": "Молоко, хлеб, яйца",
    "priority": 0,
    "status": 0,
    "categoryId": 2,
    "categoryName": "Личное",
    "createdAt": "2026-02-03T11:00:00Z",
    "dueDate": "2026-02-04T00:00:00Z",
    "completedAt": null,
    "tags": ["покупки"]
  }
]
```

### Получить задачи с фильтрацией

**По статусу:**
```http
GET /api/tasks?status=1
Authorization: Bearer {token}
```

**По категории:**
```http
GET /api/tasks?categoryId=1
Authorization: Bearer {token}
```

**По статусу и категории:**
```http
GET /api/tasks?status=0&categoryId=2
Authorization: Bearer {token}
```

### Получить задачу по ID

**Запрос:**
```http
GET /api/tasks/1
Authorization: Bearer {token}
```

**Ответ (200 OK):**
```json
{
  "taskId": 1,
  "title": "Изучить ASP.NET Core",
  "description": "Пройти официальную документацию",
  "priority": 2,
  "status": 1,
  "categoryId": 1,
  "categoryName": "Работа",
  "createdAt": "2026-02-03T10:00:00Z",
  "dueDate": "2026-02-15T00:00:00Z",
  "completedAt": null,
  "tags": ["обучение", "программирование"]
}
```

**Ошибка (404 Not Found):**
```json
{
  "message": "Задача не найдена"
}
```

### Создать новую задачу

**Запрос:**
```http
POST /api/tasks
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Написать отчет",
  "description": "Квартальный отчет по проекту",
  "priority": 2,
  "categoryId": 1,
  "dueDate": "2026-02-10T00:00:00Z",
  "tags": ["работа", "отчет"]
}
```

**Ответ (201 Created):**
```json
{
  "taskId": 3,
  "title": "Написать отчет",
  "description": "Квартальный отчет по проекту",
  "priority": 2,
  "status": 0,
  "categoryId": 1,
  "categoryName": "Работа",
  "createdAt": "2026-02-03T12:00:00Z",
  "dueDate": "2026-02-10T00:00:00Z",
  "completedAt": null,
  "tags": ["работа", "отчет"]
}
```

### Обновить задачу

**Запрос (частичное обновление):**
```http
PUT /api/tasks/1
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": 2,
  "priority": 3
}
```

**Запрос (полное обновление):**
```http
PUT /api/tasks/1
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Изучить ASP.NET Core (обновлено)",
  "description": "Пройти официальную документацию и создать проект",
  "priority": 3,
  "status": 1,
  "categoryId": 1,
  "dueDate": "2026-02-20T00:00:00Z"
}
```

**Ответ (200 OK):**
```json
{
  "taskId": 1,
  "title": "Изучить ASP.NET Core (обновлено)",
  "description": "Пройти официальную документацию и создать проект",
  "priority": 3,
  "status": 1,
  "categoryId": 1,
  "categoryName": "Работа",
  "createdAt": "2026-02-03T10:00:00Z",
  "dueDate": "2026-02-20T00:00:00Z",
  "completedAt": null,
  "tags": ["обучение", "программирование"]
}
```

### Удалить задачу

**Запрос:**
```http
DELETE /api/tasks/1
Authorization: Bearer {token}
```

**Ответ (204 No Content):**
```
(пустой ответ)
```

### Получить статистику

**Запрос:**
```http
GET /api/tasks/statistics
Authorization: Bearer {token}
```

**Ответ (200 OK):**
```json
{
  "Total": 15,
  "New": 5,
  "InProgress": 7,
  "Completed": 3,
  "Overdue": 2
}
```

## 📁 Категории

### Получить все категории

**Запрос:**
```http
GET /api/categories
Authorization: Bearer {token}
```

**Ответ (200 OK):**
```json
[
  {
    "categoryId": 1,
    "name": "Работа",
    "color": "#2196F3",
    "taskCount": 8
  },
  {
    "categoryId": 2,
    "name": "Личное",
    "color": "#4CAF50",
    "taskCount": 5
  },
  {
    "categoryId": 3,
    "name": "Учеба",
    "color": "#FF9800",
    "taskCount": 2
  }
]
```

### Создать категорию

**Запрос:**
```http
POST /api/categories
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Спорт",
  "color": "#F44336"
}
```

**Ответ (200 OK):**
```json
{
  "categoryId": 4,
  "name": "Спорт",
  "color": "#F44336",
  "taskCount": 0
}
```

### Удалить категорию

**Запрос:**
```http
DELETE /api/categories/4
Authorization: Bearer {token}
```

**Ответ (204 No Content):**
```
(пустой ответ)
```

**Ошибка (404 Not Found):**
```json
{
  "message": "Категория не найдена или является системной"
}
```

## 📊 Перечисления (Enums)

### TaskPriority (Приоритет)
```
0 - Low (Низкий)
1 - Medium (Средний)
2 - High (Высокий)
3 - Critical (Критический)
```

### TaskStatus (Статус)
```
0 - New (Новая)
1 - InProgress (В процессе)
2 - Completed (Выполнена)
3 - Postponed (Отложена)
4 - Cancelled (Отменена)
```

## 🔧 Примеры использования с cURL

### Регистрация
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "email": "test@example.com",
    "password": "Test123",
    "firstName": "Тест",
    "lastName": "Тестов"
  }'
```

### Вход
```bash
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "testuser",
    "password": "Test123"
  }'
```

### Получить задачи
```bash
curl -X GET http://localhost:5000/api/tasks \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### Создать задачу
```bash
curl -X POST http://localhost:5000/api/tasks \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Новая задача",
    "description": "Описание задачи",
    "priority": 1,
    "categoryId": 1,
    "dueDate": "2026-02-15T00:00:00Z",
    "tags": ["тест"]
  }'
```

## 🐛 Коды ошибок

| Код | Описание |
|-----|----------|
| 200 | OK - Запрос выполнен успешно |
| 201 | Created - Ресурс создан |
| 204 | No Content - Успешно, но нет содержимого |
| 400 | Bad Request - Неверный запрос |
| 401 | Unauthorized - Требуется авторизация |
| 404 | Not Found - Ресурс не найден |
| 500 | Internal Server Error - Внутренняя ошибка сервера |

## 💡 Советы

1. **Сохраните токен** после входа/регистрации для последующих запросов
2. **Токен действителен 7 дней** - после этого нужно войти заново
3. **Используйте Swagger UI** для интерактивного тестирования: http://localhost:5000/swagger
4. **Проверяйте формат дат** - используйте ISO 8601 формат
5. **Обрабатывайте ошибки** - всегда проверяйте статус код ответа

## 🔍 Тестирование в Postman

1. Импортируйте коллекцию (создайте новую коллекцию)
2. Добавьте переменную окружения `baseUrl` = `http://localhost:5000/api`
3. Добавьте переменную `token` для хранения JWT токена
4. Используйте `{{baseUrl}}` и `{{token}}` в запросах

### Пример настройки авторизации в Postman:
- Type: Bearer Token
- Token: `{{token}}`

---

**Документация актуальна для версии API:** 1.0.0  
**Дата обновления:** 03.02.2026
