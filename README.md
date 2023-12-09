#Welcome

#Clone this project using git clone
git clone https://github.com/sandipsky/dotnet-crud

#Add Dependencies
dotnet add

#Run This project 
dotnet run

#API List
#Registration[POST]
http://localhost:5000/api/Auth/register
{
  "user_name": "sandip",
  "password": "password",
  "full_name": "Sandip"
}

#Login[POST]
http://localhost:5000/api/Auth/login
{
  "user_name": "sandip",
  "password": "s3",
}

#Create Product[POST]
http://localhost:5000/api/product
{
  "id": 0,
  "name": "xyz",
  "price": 50
}

#Get all Product[GET]
http://localhost:5000/api/product/

#Get Product by id[GET]
http://localhost:5000/api/product/<id>

#Update Product[PUT]
http://localhost:5000/api/product/<id>

#Delete Product[DELETE]
http://localhost:5000/api/product/<id>

#Filter Product[GET]
http://localhost:5000/api/Product/filter?sortBy=name&sortOrder=asc&filterByName=a&minPrice=1&maxPrice=110&page=1&pageSize=10






