// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope = "type", Target = "Salient.ReliableHttpClient.Testing.TestWebRequest", Justification = "There is no current need to implement ISerializable but decorating and adding this supression obviates a lot more unnecessary code")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2229:ImplementSerializationConstructors", Scope = "type", Target = "Salient.ReliableHttpClient.Testing.TestWebRequest", Justification = "There is no current need to implement ISerializable but decorating and adding this supression obviates a lot more unnecessary code")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2229:ImplementSerializationConstructors", Scope = "type", Target = "Salient.ReliableHttpClient.Testing.TestWebReponse", Justification = "There is no current need to implement ISerializable but decorating and adding this supression obviates a lot more unnecessary code")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope = "type", Target = "Salient.ReliableHttpClient.Testing.TestWebReponse",Justification = "There is no current need to implement ISerializable but decorating and adding this supression obviates a lot more unnecessary code")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "Salient.ReliableHttpClient.Testing.TestRequestFactory.#Create(System.String)",Justification = "I just don't see how to resolve this. The code that calls Create is responsible for disposing the object.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope = "type", Target = "Salient.ReliableHttpClient.ReliableHttpException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member", Target = "Salient.ReliableHttpClient.ReliableHttpException.#PopulateFrom(System.Exception)",Justification = "?? simple using closure. i do not understand what it is they want")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Scope = "member", Target = "Salient.ReliableHttpClient.RequestInfo.#CompleteRequest(System.IAsyncResult)", Justification = "?? simple using closure. i do not understand what it is they want")]
