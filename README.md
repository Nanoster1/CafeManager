# Cafe Manager

Cafe Manager это приложение для управления меню и заказами.

## Основные функции

- Управление позициями меню
- Управление заказами

## Инструкция по сборке в Docker

1. Переходим в папку [build/docker](build/docker/)
2. Выполняем команду `docker compose --env-file .\docker-compose.env -f .\docker-compose.yaml up --build -d`
3. Переходим в браузер и вводим адрес <http://localhost:8080/swagger>
4. Теперь необходимо выполнить миграции, для этого в контроллере Migration выполняем запрос `migrate`
5. Готово