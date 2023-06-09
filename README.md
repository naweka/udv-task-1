# Тестовое задание
Написать web-приложение, которое обращается к личной странице в соц. сети «ВКонтакте», получает оттуда 5 последних постов, считает в этих постах вхождение одинаковых букв (сравнение регистро-независимое) 

Результат отсортировать по алфавиту, итоговые результаты подсчета записываются в БД (PostgreSQL). 

Информация о запуске подсчета и об его окончание должна записываться в лог файл на локальной файловой системе. 

В качестве UI для взаимодействия с backend частью использовать Swagger.

# Реализация
## API
API состоит из двух эндпоинтов: __analyzer/getPostStats__ и __analyzer/getLastPostsStats__.

Первый возволяет проанализировать конкретный пост на открытой стене личной страницы/сообщества ВКонтакте.

Второй позволяет проанализировать последние N постов (максимум 100).

## Анализ
Анализ представляет собой подсчёт каждой буквы, которая хоть раз появляется в тексте.

Вывод представляет собой JSON с буквами, отсортированными по алфавиту, и количеством этих букв.

## База данных
В бд PostgreSQL заносятся данные об id владельца поста, список id постов, дату анализа и сама статистика по буквам.

## Запуск
Перед запуском необходимо прописать данные в secrets.json в среде разработки:
```json
{
  "VkAccessToken": "vk1.a.q197YJmdCWu4UIOgOfTNrgw9CKBgOz_aWqh_jpy9cjtT4KhrqoSY738rpNuzDudojqEszG7IuNgxuFiFTBUgZruBPViRhakyZQT4pzjbDaNteyRekgknBJbxWvpPVhMe9cCo4XTNGHhQR8CTwDBnDmi6arDW92n-UxPHo4MKPU46ZPMgZMVFp_T*******************************",
  "ConnectionString": "Server=localhost;Port=5432;Database=testdb;User Id=admin;Password=admin;"
}
```

Миграции: `dotnet ef migrations add InitialCreate & dotnet ef database update`



Запуск PostgreSQL: `docker run --name postgres-db -p 5432:5432 -e POSTGRES_DB=testdb -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=password -d postgres:15.2`
