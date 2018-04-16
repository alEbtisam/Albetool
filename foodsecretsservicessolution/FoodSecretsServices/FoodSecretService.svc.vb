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
'Match Recipes to Daily Calories
    Public Function MatchRecipesDailyCalories(ByVal targetCalories As Integer) As DailyMeals Implements IFoodSecretService.MatchRecipesDailyCalories

        Dim apiUrl As String = "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/mealplans/generate"
        Dim params As String = "targetCalories=" + targetCalories.ToString() + "&timeFrame=day"
        Dim client As New WebClient

        client.Headers.Add("X-Mashape-Key", apiKey)
        client.Headers.Add("Accept", acceptHeader)
        Dim result As String = client.DownloadString(String.Format("{0}?{1}", apiUrl, params))

        Dim jsonObj As JObject = JsonConvert.DeserializeObject(Of Object)(result)

        Dim rtrnResult As New DailyMeals()
        rtrnResult.meals = New List(Of Meal)()

        If jsonObj("meals").Count > 0 Then
            For Each obj As JObject In jsonObj("meals")
                Dim mealObj As Meal = New Meal()
                mealObj.id = obj("id")
                mealObj.image = "https://spoonacular.com/recipeImages/" + obj("image").ToString
                mealObj.readyInMinutes = obj("readyInMinutes")
                mealObj.title = obj("title")
                rtrnResult.meals.Add(mealObj)
            Next
        End If

        rtrnResult.calories = jsonObj("nutrients")("calories")
        rtrnResult.carbohydrates = jsonObj("nutrients")("carbohydrates")
        rtrnResult.fat = jsonObj("nutrients")("fat")
        rtrnResult.protein = jsonObj("nutrients")("protein")

        Return rtrnResult
    End Function

    'Get Recipe Information
    Public Function GetRecipeInformation(ByVal RecipeID As Integer) As RecipeInformation Implements IFoodSecretService.GetRecipeInformation

        Dim apiUrl As String = "https://spoonacular-recipe-food-nutrition-v1.p.mashape.com/recipes/" + RecipeID.ToString() + "/information"
        Dim params As String = "includeNutrition=true"
        Dim client As New WebClient

        client.Headers.Add("X-Mashape-Key", apiKey)
        client.Headers.Add("Accept", acceptHeader)
        Dim result As String = client.DownloadString(String.Format("{0}?{1}", apiUrl, params))

        Dim jsonObj As JObject = JsonConvert.DeserializeObject(Of Object)(result)

        Dim rtrnResult As New RecipeInformation()

        rtrnResult.aggregateLikes = jsonObj("aggregateLikes")
        rtrnResult.cheap = jsonObj("cheap")
        'rtrnResult.dairyFree = jsonObj("dairyFree")
        'rtrnResult.glutenFree = jsonObj("glutenFree")
        'rtrnResult.ketogenic = jsonObj("ketogenic")
        'rtrnResult.lowFodmap = jsonObj("lowFodmap") 
        rtrnResult.servings = jsonObj("servings")
        rtrnResult.spoonacularScore = jsonObj("spoonacularScore")
        'rtrnResult.sustainable = jsonObj("sustainable")
        'rtrnResult.vegan = jsonObj("vegan")
        'rtrnResult.vegetarian = jsonObj("vegetarian")
        rtrnResult.veryHealthy = jsonObj("veryHealthy")
        rtrnResult.veryPopular = jsonObj("veryPopular")
        rtrnResult.weightWatcherSmartPoints = jsonObj("weightWatcherSmartPoints")
        'rtrnResult.whole30 = jsonObj("whole30")
        rtrnResult.title = jsonObj("title")
        rtrnResult.readyInMinutes = jsonObj("readyInMinutes")
        rtrnResult.image = jsonObj("image")

        Dim jObj As JObject = jsonObj("nutrition")
        Dim nutritionArray As JArray = jObj("nutrients")
        For Each obj As JObject In nutritionArray
            If obj("title").ToString() = "Calories" Then
                rtrnResult.Calories = obj("amount").ToString()
            End If

            If obj("title").ToString() = "Fat" Then
                rtrnResult.Fat = obj("amount").ToString()
            End If

            If obj("title").ToString() = "Carbohydrates" Then
                rtrnResult.Carbs = obj("amount").ToString()
            End If

            If obj("title").ToString() = "Protein" Then
                rtrnResult.Protein = obj("amount").ToString()
            End If
        Next

        Return rtrnResult
    End Function



End Class
