Imports System.ServiceModel
 
' NOTE: You can use the "Rename" command on the context menu to change the interface name "IFoodSecretService" in both code and config file together.
<ServiceContract()>
Public Interface IFoodSecretService

    <OperationContract()>
    Function SearchRecipeByName(ByVal RecipeName As String) As List(Of Recipe)

    <OperationContract()>
    Function SearchRecipeByIngredients(ByVal ingredients As String) As List(Of Recipe)

    <OperationContract()>
    Function SearchRecipeByNutrients(ByVal maxcalories As Integer, ByVal maxcarbs As Integer, ByVal maxfat As Integer, ByVal maxprotein As Integer) As List(Of Recipe)

    <OperationContract()>
    Function MatchRecipesDailyCalories(ByVal targetCalories As Integer) As DailyMeals

    <OperationContract()>
    Function GetRecipeInformation(ByVal RecipeID As Integer) As RecipeInformation

End Interface
