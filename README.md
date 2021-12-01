# Final Project CSharp API Heroku
**Nama : Gilang Trisetya Indrawan** \
**Kode Peserta : FSDO002ONL018** \
Link laporan: [laporanku](laporan.pdf) \
Link Github: https://github.com/ginwa123/GIC-OCBC-NISP-FINAL-PROJECT-1 \
Link Heroku: https://final-project-csharp.herokuapp.com \
Postman  [postman collection](Gilang_Final_Project_1_CSharp%20Heroku.postman_collection.json)
# API Yang dimiliki
## Auth Controller
Menggunakan JWT token sebagai sistem keamanan
### Register akun

link : https://final-project-csharp.herokuapp.com/api/auth/register \
Input body json
```json
{
    "username": "ginwa123",
    "email": "ginwa123gmail.com",
    "password": "Gilang12345!"
}
```

---
### Login akun
 link : https://final-project-csharp.herokuapp.com/api/auth/login \
Input body json
```json
{
    "email": "ginwa123gmail.com",
    "password": "Gilang12345!"
}
```
---
### Refresh Token
link : https://final-project-csharp.herokuapp.com/api/auth/refreshtoken \
Input body json
```json
{
    "token" : "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6ImE2MTY0ZGZhLTMxMTUtNGQwYy",
    "refreshtoken": "KV5MZC7P9D0e8d9e11-02cf-44d2-9e77-866088c208a0"
}
```

---
## Payment Controller
### Get All payment
link : https://final-project-csharp.herokuapp.com/api/paymentdetail \
Response berhasil
```json
{
    "message": "Get all paayment",
    "success": true,
    "data": [
        {
            "paymentDetailId": 5,
            "cardOwnerName": "Gilang Trisetya Indrawan",
            "cardNumber": 231231231,
            "expirationDate": "2021-11-30T13:36:02.881",
            "securityCode": "ksjn23snsda"
        },
        {
            "paymentDetailId": 6,
            "cardOwnerName": "Mahendra",
            "cardNumber": 23231223,
            "expirationDate": "2021-11-30T13:36:02.881",
            "securityCode": "ksjsadafgda"
        },
        {
            "paymentDetailId": 7,
            "cardOwnerName": "Bagus",
            "cardNumber": 2308654,
            "expirationDate": "2021-11-30T13:36:02.881",
            "securityCode": "lskfsaja"
        }
    ]
}
```
---
### Get Payment By id
link : https://final-project-csharp.herokuapp.com/api/paymentdetail/id \
slug : id \
Response berhasil menemukan
```json
{
    "message": "payment found",
    "success": true,
    "data": {
        "paymentDetailId": 4,
        "cardOwnerName": "Gilang Trisetya Indrawan",
        "cardNumber": 231231231,
        "expirationDate": "2021-11-30T13:36:02.881",
        "securityCode": "ksjn23snsda"
    }
}

```
---
### Create payment
link : https://final-project-csharp.herokuapp.com/api/paymentdetail \
Input body json
```json
{
  "cardOwnerName": "Maeda",
  "cardNumber": 23223,
  "expirationDate": "2021-11-30T13:36:02.881Z",
  "securityCode": "sdasdl12"
}
```
---
### Update Payment
link : https://final-project-csharp.herokuapp.com/api/paymentdetail/id \
slug : id \
Input body json
```json
{
  "cardOwnerName": "Mahendra",
  "cardNumber": 213213292642,
  "expirationDate": "2021-11-30T13:36:02.881Z",
  "securityCode": "string"
}
```
---
### Delete Payment
link : https://final-project-csharp.herokuapp.com/api/paymentdetail/id \
slug : id
Response berhasil menghapus
```json
{
    "message": "Delete payment success",
    "success": true,
    "data": null
}
```