# Добавление поддержки JWT в Asp.Net


## 1 Добавление поддержки аутентификации

Откройте проект `ApiJwtDemo`. Изучите его структуру. Запустите проект. Вы увидите, что в браузере будет путь
```
/api/secure/secret
```
и будет выведена некоторая строка.

Код данного метода вы можете найти в контроллере `SecureController`:
```cs
[Route("api/[controller]")]
[ApiController]
public class SecureController : ControllerBase
{
    [HttpGet("secret")] // /api/secure/secret
    public string SecretString() => "Защищеный метод, доступный только после аутентификации";
}
```

Наша задача: реализовать защиту данного метода, чтобы доступ к нему был только у аутентифицированного пользователя.

Сперва необходимо установить пакет:
```
Microsoft.AspNetCore.Authentication.JwtBearer
```

Далее, добавим в конвейер `app.UseAuthentication()`:
```cs
app.UseHttpsRedirection();
app.UseAuthentication(); // аутентификация
app.UseAuthorization();
app.MapControllers();
```

И внедрим сервис. Добавление аутентификации:
```cs
builder.Services.AddAuthentication();
```

Укажем схему (как и в случае с Cookies):
```cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
```

Добавим поддержку токена:
```cs
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        // options
    });
```

Зададим опции:
```cs
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // провереяем издателя
            ValidateAudience = false, // не проверяем аудиторию
            ValidateLifetime = true, // проверяем время жизни
            ValidateIssuerSigningKey = true, // проверяем ключ
            ValidIssuer = "ects" // валидный издатель токена
        };
    });
```

Укажем симметричный ключ:
```cs
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // провереяем издателя
            ValidateAudience = false, // не проверяем аудиторию
            ValidateLifetime = true, // проверяем время жизни
            ValidateIssuerSigningKey = true, // проверяем ключ
            ValidIssuer = "ects",
            IssuerSigningKey = new SymmetricSecurityKey() // здесь пока что будет ошибка компиляции
        };
    });
```

В качестве параметра нужно задать ключ. Добавим статический класс:
```cs
public static class KeyProvider
{
    private static string keyString = 
        "super_secret_string_123_poiuytrewqasdfghjklzxcvbnm";

    public static byte[] Key => Encoding.Default.GetBytes(keyString);

}
```

Используем ключ в качестве параметра для конструктора `SymmetricSecurityKey`:
```cs
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // провереяем издателя
            ValidateAudience = false, // не проверяем аудиторию
            ValidateLifetime = true, // проверяем время жизни
            ValidateIssuerSigningKey = true, // проверяем ключ
            ValidIssuer = "ects",
            IssuerSigningKey = new SymmetricSecurityKey(KeyProvider.Key)
        };
    });
```

Добавим защищенному методу аттрибут `[Authorize]`:
```cs
[Route("api/[controller]")]
[ApiController]
public class SecureController : ControllerBase
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("secret")] // /api/secure/secret
    public string SecretString() => "Защищеный метод, доступный только после аутентификации";
}
```

Запустим приложение и получим ответ `401`. Чтобы увидеть код ответа используйте Postman и выполните GET запрос.

## 2 Получение токена

Добавьте в проект контроллер Api и назовите его `UsersController`. Добавьте метод `Login`:
```cs
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Ok("TOKEN");
    }
}
```

Проверьте его работоспособность, обратившись по `/api/users/login`.

Создайте папку `DataTransfer`. Добавьте в нее класс:
```cs
public class UserDTO
{
    [StringLength(100)]
    [MinLength(3)]
    public string Username { get; set; } = string.Empty;

    [StringLength(100)]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

}
```

Это специальный класс для сериализации. Мы будем его использовать для передачи данных.

Добавим в метод контроллера параметр и изменим метод HTTP с GET на POST:
```cs
[HttpPost("login")]
public IActionResult Login(UserDTO user)
{
    return Ok("TOKEN");
}
```

Реализуем логику:
```cs
[HttpPost("login")]
public IActionResult Login(UserDTO user)
{
    if (user.Username == "admin" && user.Password == "qwerty123")
    {
        return Ok("TOKEN");
    }
    return BadRequest("Invalid username or password");
}
```

Отправим запрос с помощью POSTMAN:
![](3.png)

Получим ответ с ошибками валидации:
```js
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "traceId": "00-897c00707a8ead47624805b7eacfae11-6c54c00513606bb0-00",
    "errors": {
        "Password": [
            "The field Password must be a string or array type with a minimum length of '8'."
        ],
        "Username": [
            "The field Username must be a string or array type with a minimum length of '3'."
        ]
    }
}
```

Проверьте другие варианты, указав:
- валидные значения, но неправильные логин или пароль;
- валидные значения, правильные логин и пароль (ответом будет `TOKEN`).

Добавим метод:
```cs
private string getToken(UserDTO user)
{
    // создаем токен
    var token = new JwtSecurityToken();
    // обрабатываем его, получая строку
    var handler = new JwtSecurityTokenHandler();
    string result = handler.WriteToken(token);
    // возвращаем результат
    return result;
}
```

Изменим его, добавив параметры токена:
```cs
private string getToken(UserDTO user)
{
    // утверждения о пользователе
    List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, "Client")
    };

    // настройки ключа
    SigningCredentials credentials = new SigningCredentials(
            new SymmetricSecurityKey(KeyProvider.Key), // секретный ключ
            SecurityAlgorithms.HmacSha256); // алгоритм

    // создаем токен
    var token = new JwtSecurityToken(
            issuer: "ects",
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddHours(12),
            claims: claims,
            signingCredentials: credentials
        );
    // обрабатываем его, получая строку
    var handler = new JwtSecurityTokenHandler();
    string result = handler.WriteToken(token);
    // возвращаем результат
    return result;
}
```

Вызовем метод внутри `Login`:
```cs
[HttpPost("login")]
public IActionResult Login(UserDTO user)
{
    if (user.Username == "admin" && user.Password == "qwerty123")
    {
        return Ok(getToken(user));
    }
    return BadRequest("Invalid username or password");
}
```

Запустите, и проверьте в POSTMAN, что вы получаете токен.
![](4.png)

## 3 Использование токена

Сперва перейдите на сайт https://jwt.io/

Попробуйте вставить ваш токен в окно ввода. Исследуйте ваш токен. Сравните значения с тем, что было установлено программно.
![](5.png)

Далее, создайте запрос к методу `GET /api/secure/secret`. Во вкладке заголовков добавьте заголовок `Authorization` и укажите в нем значение `Bearer <ВАШ ТОКЕН>`. Выполните запрос.
![](6.png)

Отключите заголовок, проверьте, что без него будет код ответа `401`.

Альтернативный вариант, выбрать вкладку `Auth` и указать там `Bearer Token`:
![](7.png)

Готово! Мы успешно добавили поддержку `JSON Web Token` в проект на Asp.Net Core.


