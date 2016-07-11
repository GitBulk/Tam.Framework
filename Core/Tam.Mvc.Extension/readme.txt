How to use JsonCamelCaseResult:

public ActionResult GetPerson()
{
    return new JsonCamelCaseResult(new Person { FirstName = "Joe", LastName = "Public" }, JsonRequestBehavior.AllowGet)};
}