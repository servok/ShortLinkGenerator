# ShortLinkGenerator
Для запуска в конфиге необходимо указать ключ для IronBarcodeGenerator, папку для сохранения сгенерированных QR и подключение к БД.
UI часть реализована просто в виде статичного html файла и доступна в wwwroot/client/Index.html

Демонстрационная версия доступна по адресу http://servok.ru:4657/client/Index.html

Для получения короткой ссылки без UI - отправить POST на http://servok.ru:4657/ ссылка должна быть в теле запроса (contentType: 'application/json')

Для редиректа GET http://servok.ru:4657/{token}
