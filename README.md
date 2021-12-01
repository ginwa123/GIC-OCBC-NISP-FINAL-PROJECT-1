# Final Project CSharp API Heroku

Link Github: https://github.com/ginwa123/GIC-OCBC-NISP-FINAL-PROJECT-1 \
Link Heroku: https://final-project-csharp.herokuapp.com \
Postman Collection: https://www.getpostman.com/collections/3384626b8d9da017b4fe
## API Yang dimiliki
---

### Auth
JWT TOKEN berlaku 2 menit \
Create account : https://final-project-csharp.herokuapp.com/api/auth/register \
Sample payload
```json
{
    "username": "ginwa123",
    "email": "ginwa123gmail.com",
    "password": "Gilang12345!"
}
```

 Login account : https://final-project-csharp.herokuapp.com/api/auth/login \
Sample payload
```json
{
    "email": "ginwa123gmail.com",
    "password": "Gilang12345!"
}
```

3.RefreshToken : https://final-project-csharp.herokuapp.com/api/auth/refreshtoken \
Sample payload
```json
{
    "token" : "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6ImE2MTY0ZGZhLTMxMTUtNGQwYy1iMGNjLWNiOGU4NmRhYTE5MiIsImVtYWlsIjoiZ2lud2ExMjNnbWFpbC5jb20iLCJzdWIiOiJnaW53YTEyM2dtYWlsLmNvbSIsImp0aSI6IjhlNWY1Nzk1LTE1NzYtNDE3Yi1iNTQ0LTk5YjBhMzVhNzE1NCIsIm5iZiI6MTYzODI4NzIwOCwiZXhwIjoxNjM4Mjg3MzI4LCJpYXQiOjE2MzgyODcyMDh9.JC_sY773bSz9JsuilkgDE5qT6g1avMI5699CzDc60ig",
    "refreshtoken": "KV5MZC7P9D0e8d9e11-02cf-44d2-9e77-866088c208a0"
}
```

### Payment
---
Get all payment : https://final-project-csharp.herokuapp.com/api/paymentdetail

Get payment by id : https://final-project-csharp.herokuapp.com/api/paymentdetail/1

Create Payment : https://final-project-csharp.herokuapp.com/api/paymentdetail \
Sample payload
```json
{
  "cardOwnerName": "Maeda",
  "cardNumber": 23223,
  "expirationDate": "2021-11-30T13:36:02.881Z",
  "securityCode": "sdasdl12"
}
```

Update Payment : https://final-project-csharp.herokuapp.com/api/paymentdetail/1 \
Sample payload
```json
{
  "cardOwnerName": "edited",
  "cardNumber": 213213,
  "expirationDate": "2021-11-30T13:36:02.881Z",
  "securityCode": "string"
}
```
Delete Payment : https://final-project-csharp.herokuapp.com/api/paymentdetail/1