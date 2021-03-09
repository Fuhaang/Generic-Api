﻿using Xunit;


namespace Entities.UnitTests
{
    public class EntityTests
    {
        [Fact]
        public void Entity_HasID()
        {
            Common.HasProperty(typeof(Entity), "Id");
        }

        [Fact]
        public void EntityID_HasCorrectType()
        {
            Common.PropertyType(typeof(long), typeof(Entity), "Id");
        }
    }
}
