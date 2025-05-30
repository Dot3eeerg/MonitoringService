# Описание
Тестовое задание выполненное для компании infotecs. По ТЗ были выполнены все задания, кроме генерации схемы API.

## Развёртка докера
1. **Сборка докер-контейнера:**
    ```bash
    docker-compose build
    ```
2. Запуск контейнера:**
    ```bash
    docker-compose up
    ```

Клиентская часть лежит по адресу [http://localhost:5000](http://localhost:5000)

Пользователи могут просматривать все доступные записи, а также получать данные по конкретному устройству. Дополнительно предусмотрен опциональный выбор имени пользователя для фильтра данных. Также можно удалять отдельно выбранные записи.

Бэкап базы данных делается с помощью POST-запроса. Пример:
```
http://localhost:5001/api/backup/create
```
Файл сохранится проекте WebApi в директории Backups.

Для загрузки бэкапа на работающий сервер достаточно указать имя файла. Пример:
```
http://localhost:5001/api/backup/restore?fileName=backup_20250116162006.json
```

Логи выводятся как в терминал, так и в файл. В проекте WebApi в директории Logs можно найти все логи.

