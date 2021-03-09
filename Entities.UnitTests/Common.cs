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
        /// Vérifie que l'entité possède bien la propriété passée en paramètre.
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        public static void HasProperty(Type typeObjet, string nomPropriete)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            Assert.NotNull(property);
        }

        /// <summary>
        /// Vérifie que l'attribut de l'entité a l'annotation [Display(Name = "xxx")] avec la valeur attendue.
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        /// <param name="chaineAttendue">valeur attendue pour l'affichage de cette propriété</param>
        public static void DisplayAttribute(Type typeObjet, string nomPropriete, string chaineAttendue)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            var annotation = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            Assert.NotNull(annotation);
            Assert.Equal(chaineAttendue, annotation.Name);
        }
        /// <summary>
        /// Vérifie que l'attribut de l'entité a l'annotation [MaxLength(xx)] avec la longueur attendue.
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        /// <param name="max">longueur maximum</param>
        public static void MaxLengthAttribute(Type typeObjet, string nomPropriete, int max)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            var annotation = (MaxLengthAttribute)property.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault();
            Assert.NotNull(annotation);
            Assert.Equal(max, annotation.Length);
        }

        /// <summary>
        /// Vérifie que l'attribut de l'entité a l'annotation [MinLength(xx)] avec la longueur attendue.
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        /// <param name="min">longueur minimum</param>
        public static void MinLengthAttribute(Type typeObjet, string nomPropriete, int min)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            var annotation = (MinLengthAttribute)property.GetCustomAttributes(typeof(MinLengthAttribute), false).FirstOrDefault();
            Assert.NotNull(annotation);
            Assert.Equal(min, annotation.Length);
            Assert.Equal(min, annotation.Length);
        }

        /// <summary>
        /// Vérifie que l'attribut de l'entité a l'annotation [Required].
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        public static void RequiredAttribute(Type typeObjet, string nomPropriete)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            var annotation = (RequiredAttribute)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
            Assert.NotNull(annotation);
        }

        /// <summary>
        /// Vérifie que l'attribut de l'entité n'a pas l'annotation [Url].
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        public static void HasNotValidationUrlAttribute(Type typeObjet, string nomPropriete)
        {
            var property = typeObjet.GetProperty(nomPropriete);
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
