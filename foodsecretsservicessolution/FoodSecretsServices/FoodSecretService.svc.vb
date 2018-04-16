Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Dynamic
Imports System.IO


Public Class FoodSecretService
    Implements IFoodSecretService

    Public apiKey As String = "pRq2hrCLoGmshj3U1W1j09AMOshVp1YK0rvjsnTauYOsE8frW4"
    Public acceptHeader As String = "application/json"

    'Search for recipe by Name
    Public Function SearchRecipeByName(ByVal RecipeName As String) As List(Of Recipe) Implements IFoodSecretService.SearchRecipeByName

        Dim apiUrl As String = "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/search"  ' "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/autocomplete"
        Dim params As String = "number=10&query=" + RecipeName.ToString()
        Dim client As New WebClient

        client.Headers.Add("X-Mashape-Key", apiKey)
        client.Headers.Add("Accept", acceptHeader)
        Dim result As String = client.DownloadString(String.Format("{0}?{1}", apiUrl, params))

        Dim resObj As JObject = JsonConvert.DeserializeObject(Of Object)(result)

        Dim jsonArray As JArray = resObj("results") 'JsonConvert.DeserializeObject(Of Object)(result)

        Dim rtrnResult As New List(Of Recipe)
        If jsonArray.Count > 0 Then
            For Each obj As JObject In jsonArray
                Dim recipeObj As Recipe = New Recipe()
                recipeObj.id = obj("id")
                recipeObj.title = obj("title")
                recipeObj.image = obj("image")

                rtrnResult.Add(recipeObj)
            Next
        End If

        Return rtrnResult
    End Function

    'Search for recipe by ingredients
    Public Function SearchRecipeByIngredients(ByVal ingredients As String) As List(Of Recipe) Implements IFoodSecretService.SearchRecipeByIngredients

        ingredients = ingredients.Replace(",", "%2C")
        ingredients = ingredients.Replace(" ", "")

        Dim apiUrl As String = "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/findByIngredients"
        Dim params As String = "fillIngredients=false&ingredients=" + ingredients + "&limitLicense=false&number=10&ranking=1"
        Dim client As New WebClient

        client.Headers.Add("X-Mashape-Key", apiKey)
        client.Headers.Add("Accept", acceptHeader)
        Dim result As String = client.DownloadString(String.Format("{0}?{1}", apiUrl, params))

        Dim jsonArray As JArray = JsonConvert.DeserializeObject(Of Object)(result)

        Dim rtrnResult As New List(Of Recipe)
        If jsonArray.Count > 0 Then
            For Each obj As JObject In jsonArray
                Dim recipeObj As Recipe = New Recipe()
                recipeObj.id = obj("id")
                recipeObj.image = obj("image")
                recipeObj.imageType = obj("imageType")
                recipeObj.title = obj("title")
                rtrnResult.Add(recipeObj)
            Next
        End If

        Return rtrnResult
    End Function

    'Search for recipe by nutrients
    Public Function SearchRecipeByNutrients(ByVal maxcalories As Integer, ByVal maxcarbs As Integer, ByVal maxfat As Integer, ByVal maxprotein As Integer) As List(Of Recipe) Implements IFoodSecretService.SearchRecipeByNutrients

        Dim apiUrl As String = "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/findByNutrients"
        Dim params As String = "maxcalories=" + maxcalories.ToString() + "&maxcarbs=" + maxcarbs.ToString() + "&maxfat=" + maxfat.ToString() + "&maxprotein=" + maxprotein.ToString() + "&mincalories=0&minCarbs=0&minfat=0&minProtein=0&number=10&offset=0&random=true"
        Dim client As New WebClient

        client.Headers.Add("X-Mashape-Key", apiKey)
        client.Headers.Add("Accept", acceptHeader)
        Dim result As String = client.DownloadString(String.Format("{0}?{1}", apiUrl, params))

        Dim jsonArray As JArray = JsonConvert.DeserializeObject(Of Object)(result)

        Dim rtrnResult As New List(Of Recipe)
        If jsonArray.Count > 0 Then
            For Each obj As JObject In jsonArray
                Dim recipeObj As Recipe = New Recipe()
                recipeObj.id = obj("id")
                recipeObj.image = obj("image")
                recipeObj.imageType = obj("imageType")
                recipeObj.title = obj("title")
                recipeObj.calories = obj("calories")
                recipeObj.protein = obj("protein")
                recipeObj.fat = obj("fat")
                recipeObj.carbs = obj("carbs")
                rtrnResult.Add(recipeObj)
            Next
        End If

        Return rtrnResult
    End Function



End Class
