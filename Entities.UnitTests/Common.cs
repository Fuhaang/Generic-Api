using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Entities.UnitTests
{
    public static class Common
    {
        /// <summary>
        /// Checks that the entity has the property passed as a parameter.
        /// </summary>
        /// <param name="objectType">entity type</param>
        /// <param name="propertyName">entity property name</param>
        public static void HasProperty(Type objectType, string propertyName)
        {
            var property = objectType.GetProperty(propertyName);
            Assert.NotNull(property);
        }

        /// <summary>
        /// Checks that the entity attribute has the annotation [Display (Name = "expectedString")] with the expected value.
        /// </summary>
        /// <param name="objectType">entity type</param>
        /// <param name="propertyName">entity property name</param>
        /// <param name="expectedString">expected value for the display of this property</param>
        public static void DisplayAttribute(Type objectType, string propertyName, string expectedString)
        {
            var property = objectType.GetProperty(propertyName);
            var annotation = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            Assert.NotNull(annotation);
            Assert.Equal(expectedString, annotation.Name);
        }
        /// <summary>
        /// Checks that the entity attribute has the annotation [MaxLength (max)] with the expected length.
        /// </summary>
        /// <param name="objectType">entity type</param>
        /// <param name="propertyName">entity property name</param>
        /// <param name="max">maximum length</param>
        public static void MaxLengthAttribute(Type objectType, string propertyName, int max)
        {
            var property = objectType.GetProperty(propertyName);
            var annotation = (MaxLengthAttribute)property.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault();
            Assert.NotNull(annotation);
            Assert.Equal(max, annotation.Length);
        }

        /// <summary>
        /// Checks that the entity attribute has the annotation [MinLength(min)] with the expected length.
        /// </summary>
        /// <param name="objectType">entity type</param>
        /// <param name="propertyName">entity property name</param>
        /// <param name="min">minimum lenght</param>
        public static void MinLengthAttribute(Type objectType, string propertyName, int min)
        {
            var property = objectType.GetProperty(propertyName);
            var annotation = (MinLengthAttribute)property.GetCustomAttributes(typeof(MinLengthAttribute), false).FirstOrDefault();
            Assert.NotNull(annotation);
            Assert.Equal(min, annotation.Length);
            Assert.Equal(min, annotation.Length);
        }

        /// <summary>
        /// Checks that the feature attribute has the [Required] annotation.
        /// </summary>
        /// <param name="objectType">entity type</param>
        /// <param name="propertyName">entity property name</param>
        public static void RequiredAttribute(Type objectType, string propertyName)
        {
            var property = objectType.GetProperty(propertyName);
            var annotation = (RequiredAttribute)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
            Assert.NotNull(annotation);
        }

        /// <summary>
        /// Checks that the entity attribute does not have the [Url] annotation.
        /// </summary>
        /// <param name="objectType">entity type</param>
        /// <param name="propertyName">entity property name</param>
        public static void HasNotValidationUrlAttribute(Type objectType, string propertyName)
        {
            var property = objectType.GetProperty(propertyName);
            var annotation = (UrlAttribute)property.GetCustomAttributes(typeof(UrlAttribute), false).FirstOrDefault();
            Assert.Null(annotation);
        }

        /// <summary>
        /// Test if <paramref name="propName"/> from <paramref name="objectType"/> is of type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Which type should <paramref name="propName"/> be.</param>
        /// <param name="objectType">Object which contains <paramref name="propName"/>.</param>
        /// <param name="propName">Property name from <paramref name="objectType"/>.</param>
        public static void PropertyType(Type type, Type objectType, string propName)
        {
            var property = objectType.GetProperty(propName);
            var propType = property.PropertyType;

            propType.FullName.Should().Be(type.FullName);
        }
    }
}
