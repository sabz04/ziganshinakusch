using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ziganshinakusch.Model;

namespace UnitTests
{
    //ВАЖНО!
    //перед тестами: добавить в бд пользователя с логином 2 и логином 2
    //перед тестами: добавить в бд пользователя с логином 3 и логином 3
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void LoginTest1()
        {
            var login = "2";
            var pass = "2";
            var user_expected = Opers.LogMethod(login, pass);
            using(var db = new DbModelContainer())
            {
                var user_actual = db.Users.FirstOrDefault(x => x.Login == login && x.Password == pass);
                Assert.AreEqual(user_expected.Login, user_actual.Login);
            }
            
        }
        [TestMethod]
        public void LoginTest2()
        {
            var login = "3";
            var pass = "3";
            var user_expected = Opers.LogMethod(login, pass);
            using (var db = new DbModelContainer())
            {
                var user_actual = db.Users.FirstOrDefault(x => x.Login == login && x.Password == pass);
                Assert.AreEqual(user_expected.Login, user_actual.Login);
            }
        }
        [TestMethod]
        public void LoginTestForNull3()
        {
            var login = "4342344354534534";
            var pass = "423423434534535";
            var user_expected = Opers.LogMethod(login, pass);
            using (var db = new DbModelContainer())
            {
                User user_actual = null;
                Assert.AreEqual(user_expected, user_actual);
            }
        }
        [TestMethod]
        public void LoginTestForNull4()
        {
            var login = "2";
            var pass = "508r8302924";
            var user_expected = Opers.LogMethod(login, pass);
            using (var db = new DbModelContainer())
            {
                User user_actual = null;
                Assert.AreEqual(user_expected, user_actual);
            }
        }
        [TestMethod]
        public void RegTestNew1()
        {
            var login = "";
            var pass = "";
            Assert.IsFalse(Opers.RegMethod(login, pass));

        }
        [TestMethod]
        public void RegTestNew2()
        {
            var login = "1";
            var pass = "213123";
            Assert.IsFalse(Opers.RegMethod(login, pass));
        }
        [TestMethod]
        public void RegTestNew3()
        {
            var login = "213123";
            var pass = "213123";
            Assert.IsFalse(Opers.RegMethod(login, pass));
        }
        [TestMethod]
        public void RegTestForExist4()
        {
            var login = "2";
            var pass = "2";
            Assert.IsFalse(Opers.RegMethod(login, pass));

        }
        [TestMethod]
        public void RegTestForExist5()
        {
            var login = "3";
            var pass = "3";
            Assert.IsFalse(Opers.RegMethod(login, pass));
        }
        //Тест на полный доступ(т.е введены все данные)
        [TestMethod]
        public void TestForUserFullAccess_NO()
        {
            var login = "2";
            var pass = "2";
            var user = Opers.LogMethod(login, pass);
            Assert.IsFalse(Opers.IsFullAccess(user));
        }
        [TestMethod]
        public void TestForUserFullAccess_YES()
        {
            var login = "zilya";
            var pass = "12345";
            var user = Opers.LogMethod(login, pass);
            Assert.IsTrue(Opers.IsFullAccess(user));
        }
        //Тест на то, имеет ли пользователь вещи в корзине
        [TestMethod]
        public void TestForUserHasGoodInBucket_NO()
        {
            var login = "2";
            var pass = "2";
            var user = Opers.LogMethod(login, pass);
            Assert.IsFalse(Opers.IsHasGoodInBucket(user));
        }
        [TestMethod]
        public void TestForUserHasGoodInBucket_YES()
        {
            var login = "zilya";
            var pass = "12345";
            var user = Opers.LogMethod(login, pass);
            Assert.IsTrue(Opers.IsHasGoodInBucket(user));
        }
        [TestMethod]
        public void TestForUserHasOrder_NO()
        {
            var login = "zilya";
            var pass = "12345";
            var user = Opers.LogMethod(login, pass);
            Assert.IsFalse(Opers.IsHasOrder(user));
        }
        [TestMethod]
        public void TestForUserHasOrder_YES()
        {
            var login = "userTester";
            var pass = "iamTesterUser";
            var user = Opers.LogMethod(login, pass);
            Assert.IsTrue(Opers.IsHasOrder(user));
        }
        [TestMethod]
        public void TestForNullAccess()
        {
            var login = "zilya";
            var pass = "12345222";
            var user = Opers.LogMethod(login, pass);
            Assert.IsFalse(Opers.IsFullAccess(user));
        }
        [TestMethod]
        public void TestForAddNullGoodToBucket()
        {
            var login = "zilya";
            var pass = "12345";
            var user = Opers.LogMethod(login, pass);
            Assert.IsFalse(Opers.AddGoodToBucket(user, null));

        }
        [TestMethod]
        public void TestForAddGoodToNullUser()
        {

            Assert.IsFalse(Opers.AddGoodToBucket(null, new Good
            {
                File =null ,
                Info= "testgood",
                Price= 0,
                Name = "testgood",
                Type="testtype"
            }));

        }
        [TestMethod]
        public void TestForAddGoodTo_INCORRECT_User()
        {

            var login = "nonameuser";
            var pass = "iamtestuser";
            var user = new User
            {
                Login = login,
                Password = pass
            };
            using(var db = new DbModelContainer())
            {
                Assert.IsFalse(Opers.AddGoodToBucket(user,
                    db.GoodSet.Where(x=>x.Bucket==null).First()));

            }
        }
        [TestMethod]
        public void TestForAdd_INCORRECT_GoodToUser()
        {
            var login = "zilya";
            var pass = "12345";
            var user = Opers.LogMethod(login, pass);
            var good = new Good
            {
                File = null,
                Info = "testgood",
                Price = 0,
                Name = "testgood",
                Type = "testtype"

            };
            Assert.IsFalse(Opers.AddGoodToBucket(user, null));

        }
        [TestMethod]
        public void TestForAdd_EXIST_IN_OTHER_BUCKET_GoodToUser()
        {
            var login = "zilya";
            var pass = "12345";
            var user = Opers.LogMethod(login, pass);
            using(var db = new DbModelContainer())
            {
                Assert.IsFalse(Opers.AddGoodToBucket(user,db.GoodSet.Where(
                    x=>x.Bucket != null).FirstOrDefault()));
            }

        }
        [TestMethod]
        public void TestForEditNullUser()
        {
            User user = null;
            var login = "";
            var pass = "";
            var email = "";
            var phone = "";
            var card = "";
            var full_name = "";
            var user_result = Opers.EditUser(678678678, login,pass,phone,email, card, full_name);
            Assert.AreEqual(user, user_result);

        }
        [TestMethod]
        public void TestForEditUser_WithEmptyValue()
        {
            using(var db = new DbModelContainer())
            {
                var user = db.Users.FirstOrDefault(x => x.Login == "2");
                var login = "";
                var pass = "";
                var email = "";
                var phone = "";
                var card = "";
                var full_name = "";
                User user_exp = null;
                var user_result = Opers.EditUser(user.Id, login, pass, phone, email, card, full_name);
                Assert.AreEqual(user_exp, user_result);
            } 
        }
        [TestMethod]
        public void TestForEditUser_With_ONE_EmptyValue()
        {
            using (var db = new DbModelContainer())
            {
                var user = db.Users.FirstOrDefault(x => x.Login == "2");
                var login = "2";
                var pass = "2";
                var email = "";
                var phone = "+67808533";
                var card = "1123 2222 3333 1111";
                var full_name = "I am test user";
                User user_exp = null;
                var user_result = Opers.EditUser(user.Id, login, pass, phone, email, card, full_name);
                Assert.AreEqual(user_exp, user_result);
            }


        }
        [TestMethod]
        public void TestForEditUser_With_NEGATIVE_ID()
        {
            using (var db = new DbModelContainer())
            {
                var id = -1;
                var login = "2";
                var pass = "2";
                var email = "ddd";
                var phone = "+67808533";
                var card = "1123 2222 3333 1111";
                var full_name = "I am test user";
                User user_exp = null;
                var user_result = Opers.EditUser(id, login, pass, phone, email, card, full_name);
                Assert.AreEqual(user_exp, user_result);
            }

        }
        [TestMethod]
        public void TestForRemoveGood_From_Bucket__NULL_Good()
        {
            using (var db = new DbModelContainer())
            {
                var login = "zilya";
                var pass = "12345";
                var user = Opers.LogMethod(login, pass);
                Assert.IsFalse(Opers.RemoveGoodFromBucket(user, null));
            }

        }
        [TestMethod]
        public void TestForRemoveGood_From_Bucket__NULL__User_and_Good()
        {
            using (var db = new DbModelContainer())
            {
                Assert.IsFalse(Opers.RemoveGoodFromBucket(null, null));
            }

        }
        [TestMethod]

        public void TestForRemove_ANOTHER_Good_From_BucketUser_()
        {
            using (var db = new DbModelContainer())
            {
                var login = "zilya";
                var pass = "12345";
                var user = Opers.LogMethod(login, pass);
                Assert.IsFalse(Opers.RemoveGoodFromBucket(user, db.GoodSet
                    .Where(x => x.Bucket != null).FirstOrDefault()));
            }

        }
        [TestMethod]

        public void TestFor_ADMIN_Add_New_Good_Without_Name()
        {
            using (var db = new DbModelContainer())
            {
                var name = "";
                byte[] _data = null;
                var price = "0";
                var info = "0";
                var type = "test";
                Assert.IsFalse(Opers.AddNewGood(name, price, info, type, _data));
            }

        }
        [TestMethod]
        public void TestFor_ADMIN_Add_New_Good_Price_Not_Number()
        {
            using (var db = new DbModelContainer())
            {
                var name = "sdf";
                byte[] _data = null;
                var price = "0sdf";
                var info = "0";
                var type = "test";
                Assert.IsFalse(Opers.AddNewGood(name, price, info, type, _data));
            }

        }

    }
}
