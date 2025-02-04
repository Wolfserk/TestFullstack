# Тестовое задание.

Проект представляет собой тестовое задание, включающее backend-часть на .NET 9 и frontend-часть на Vue3. Приложение поддерживает две роли пользователей: менеджера и заказчика, с различными уровнями доступа и функционалом.

## Используемые технологии

- **Backend**: .NET 9, ASP.NET Core, EF Core (Code First)
- **СУБД**: MS SQL Server
- **Frontend**: Node.js, Vue3, TypeScript, Pinia

## Настройка и запуск

### Backend


1. **Удаление папки Migrations** (если она существует) из TestFullstack.Server.
2. **Создание миграции**:
   - Откройте Package Manager Console.
   - Выполните команду:
      ```bash
      add-migration Init
3. **Применение миграции**:
   - В той же консоли выполните:
    ```bash
    update-database
4. **Запуск приложения**:
   - Запустите проект через Visual Studio.
  
   P.S. при необходимости можно изменить строку подключения к БД в файле appsettings.json:   
_"ConnectionStrings": {
  "DefaultConnection": "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=TFSDB222;Integrated Security=True;Pooling=False;Multiple Active Result Sets=True;Encrypt=False"
}_

### Frontend

1. Установите зависимости (при необходимости):
   ```bash
   npm install


Первому созданному аккаунту выдаётся роль менеджера. Все последующие аккаунты будут создаваться с ролью заказчика, но через панель управления менеджер может изменить роль или создать аккаунт с выбранной ролью.

## Функционал приложения

### Роли в системе

В системе предусмотрено две роли:
1. **Менеджер**
2. **Заказчик**

### Функционал для роли Менеджера

1. Управление товарами:
   - Добавление, редактирование и удаление товаров.
2. Управление пользователями:
   - Добавление, редактирование и удаление пользователей.
3. Управление заказами:
   - Подтверждение заказа (установка даты доставки и статуса "Выполняется").
   - Закрытие заказа (установка статуса "Выполнен").

### Функционал для роли Заказчика

1. Просмотр каталога товаров.
2. Добавление товаров в корзину.
3. Оформление заказа:
   - Установка даты создания и статуса "Новый".
4. Просмотр статуса заказов:
   - Возможность фильтрации заказов по статусу.
5. Удаление заказа:
   - Возможно до тех пор, пока заказ не перешел в статус "Выполняется".


