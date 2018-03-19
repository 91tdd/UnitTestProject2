using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        private const int ExistOrderId = 91;

        [TestMethod]
        public void when_order_exist_should_update()
        {
            var log = Substitute.For<ILog>();
            var foo = Substitute.For<IFoo>();
            foo.CheckOrderExist(Arg.Any<int>(), Arg.Any<Action>(), Arg.Invoke());

            var sut = new ProdCode(foo, log);
            sut.DoStuff(ExistOrderId);

            log.Received(1).Save(Arg.Is<string>(x => x.Contains("update")));
        }
    }

    internal class ProdCode
    {
        private readonly IFoo _foo;
        private readonly ILog _log;

        public ProdCode(IFoo foo, ILog log)
        {
            _foo = foo;
            _log = log;
        }

        public void DoStuff(int orderId)
        {
            _foo.CheckOrderExist(orderId, this.Insert, this.Update);
        }

        private void Update()
        {
            _log.Save("action should be update");
        }

        private void Insert()
        {
            _log.Save("action should be insert");
        }
    }

    public interface ILog
    {
        void Save(string content);
    }

    public interface IFoo
    {
        void CheckOrderExist(int orderId, Action insert, Action update);
    }
}