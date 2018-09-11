namespace csharp ThriftTest1.Contract

service UserService{
    SaveResult Save(1:User user)
    User Get(1:i32 id)
    list<User> GetAll()
}

enum SaveResult {  
    SUCCESS = 0,  
    FAILED = 1,  
}

struct User {
    1: required i64 Id;
    2: required string Name;
    3: required i32 Age;
    4: optional bool IsVIP;
    5: optional string Remark;
}

service CalcService{  
   i32 Add(1:i32 i1,2:i32 i2)
} 
