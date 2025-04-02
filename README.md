# ArbitrajTestApi

API для отслеживания и расчета арбитражных возможностей между квартальными и биквартальными фьючерсами на Binance.

## Функциональность

- Отслеживание пар фьючерсов (квартальный и биквартальный)
- Расчет арбитражных возможностей каждую минуту
- Хранение исторических данных
- API для полуения текущих и исторических данных
- Автоматическая инициализация исторических данных при первом запуске
- Логирование с использоанием Serilog и PostgreSQL
- Мониторинг задач с помощью Hangfire

## Установка и запуск

### 1. Настройка базы данных

Отредактируйте файл `appsettings.json` с настройками подключения к базе данных(если запускаете локально если в контейнере то ничего редактировать не надо):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=arbitraj_test;Username=postgres;Password=your_password"
  }
}
```

## API Эндпоинты

### Получение последних данных арбитража

```
GET /api/arbitrage/latest
```

### Получение исторических данных

```
GET /api/arbitrage/historical
```

### Добавление отслеживаемой пары

```http
POST /api/arbitrage/tracked-pairs

Body:
{
  "id": 0,
  "quarterFutureSymbol": "BTCUSDT_250627",
  "biQuarterFutureSymbol": "BTCUSDT_250926",
  "isNew": true,
  "lastDateOfEntry": "0001-01-01T00:00:00"
}
```

## Структура проекта

```
ArbitrajTestApi/
├── Controllers/          # Контроллеры API
├── Data/                # Классы для работы с базой данных
├── Jobs/               # Периодические задачи
├── Models/             # Модели данных
├── Repositories/       # Репозитории
└── Services/           # Сервисы
```

## Логирование

Логи сохраняются в базу данных PostgreSQL в таблице `Logs`. Используется Serilog для логирования.

## Мониторинг задач

Для мониторинга периодических задач доступен Hangfire Dashboard по адресу `/hangfire`.

## Документация API

Документация API доступна по адресу `/swagger`.

