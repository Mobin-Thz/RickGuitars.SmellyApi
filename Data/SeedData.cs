using RickGuitars.SmellyApi.Models;

namespace RickGuitars.SmellyApi.Data;

public static class SeedData
{
    public static void Initialize(RickGuitarsDbContext db)
    {
        if (db.Products.Any())
            return;

        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                SerialNumber = "V95693",
                ProductType = "Guitar",
                Name = "Fender Stratocaster",
                Description = "Classic electric guitar with bright tone and comfortable body.",
                Builder = "Fender",
                Model = "Stratocaster",
                InstrumentType = "Electric",
                BackWood = "Alder",
                TopWood = "Alder",
                NumStrings = 6,
                Price = 1499.95m,
                StockQuantity = 5,
                IsFeatured = true
            },
            new Product
            {
                Id = 2,
                SerialNumber = "V9512",
                ProductType = "Guitar",
                Name = "Gibson Les Paul Standard",
                Description = "Iconic electric guitar with mahogany body and maple top.",
                Builder = "Gibson",
                Model = "Les Paul",
                InstrumentType = "Electric",
                BackWood = "Mahogany",
                TopWood = "Maple",
                NumStrings = 6,
                Price = 2499.95m,
                StockQuantity = 3,
                IsFeatured = true
            },
            new Product
            {
                Id = 3,
                SerialNumber = "X99876",
                ProductType = "Guitar",
                Name = "Martin D-18",
                Description = "Warm acoustic guitar for professional recording and live performance.",
                Builder = "Martin",
                Model = "D-18",
                InstrumentType = "Acoustic",
                BackWood = "Mahogany",
                TopWood = "Adirondack",
                NumStrings = 6,
                Price = 2999.95m,
                StockQuantity = 2,
                IsFeatured = false
            },
            new Product
            {
                Id = 4,
                SerialNumber = "T814CE",
                ProductType = "Guitar",
                Name = "Taylor 814ce",
                Description = "Premium acoustic-electric guitar with rosewood back and spruce top.",
                Builder = "Taylor",
                Model = "814ce",
                InstrumentType = "Acoustic",
                BackWood = "Rosewood",
                TopWood = "Spruce",
                NumStrings = 6,
                Price = 3499.95m,
                StockQuantity = 4,
                IsFeatured = true
            },
            new Product
            {
                Id = 5,
                SerialNumber = "M-F5G-001",
                ProductType = "Mandolin",
                Name = "Gibson F-5G Mandolin",
                Description = "Professional mandolin with classic bluegrass sound.",
                Builder = "Gibson",
                Model = "F-5G",
                InstrumentType = "Acoustic",
                BackWood = "Maple",
                TopWood = "Spruce",
                Style = "F",
                Price = 5999.95m,
                StockQuantity = 1,
                IsFeatured = true
            },
            new Product
            {
                Id = 6,
                SerialNumber = "M-A9-002",
                ProductType = "Mandolin",
                Name = "Eastman MD315 Mandolin",
                Description = "Affordable carved-top mandolin for students and performers.",
                Builder = "Eastman",
                Model = "MD315",
                InstrumentType = "Acoustic",
                BackWood = "Maple",
                TopWood = "Spruce",
                Style = "A",
                Price = 899.95m,
                StockQuantity = 6,
                IsFeatured = false
            },
            new Product
            {
                Id = 7,
                SerialNumber = "B-JAZZ-001",
                ProductType = "Bass",
                Name = "Fender Jazz Bass",
                Description = "Electric bass with classic punchy tone.",
                Builder = "Fender",
                Model = "Jazz Bass",
                InstrumentType = "Electric",
                BackWood = "Alder",
                TopWood = "Alder",
                NumStrings = 4,
                Price = 1299.95m,
                StockQuantity = 4,
                IsFeatured = false
            },
            new Product
            {
                Id = 8,
                SerialNumber = "B-SR505-001",
                ProductType = "Bass",
                Name = "Ibanez SR505",
                Description = "Modern five-string electric bass.",
                Builder = "Ibanez",
                Model = "SR505",
                InstrumentType = "Electric",
                BackWood = "Mahogany",
                TopWood = "Maple",
                NumStrings = 5,
                Price = 999.95m,
                StockQuantity = 5,
                IsFeatured = false
            },
            new Product
            {
                Id = 9,
                SerialNumber = "ACC-STR-001",
                ProductType = "Accessory",
                Name = "Elixir Guitar Strings",
                Description = "Long-lasting coated acoustic guitar strings.",
                Builder = "Elixir",
                Model = "Nanoweb",
                AccessoryCategory = "Strings",
                Price = 19.99m,
                StockQuantity = 50,
                IsFeatured = false
            },
            new Product
            {
                Id = 10,
                SerialNumber = "ACC-CASE-001",
                ProductType = "Accessory",
                Name = "Premium Guitar Case",
                Description = "Hard shell case for acoustic and electric guitars.",
                Builder = "RickGuitars",
                Model = "ProCase",
                AccessoryCategory = "Case",
                Price = 199.99m,
                StockQuantity = 12,
                IsFeatured = false
            }
        };

        var customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                FullName = "Alex Johnson",
                Email = "alex@example.com",
                PhoneNumber = "555-0101",
                DefaultShippingAddress = "12 Music Street, Nashville",
                IsVip = true,
                CustomerLevel = "VIP"
            },
            new Customer
            {
                Id = 2,
                FullName = "Maria Garcia",
                Email = "maria@example.com",
                PhoneNumber = "555-0202",
                DefaultShippingAddress = "44 Studio Avenue, Austin",
                IsVip = false,
                CustomerLevel = "Regular"
            },
            new Customer
            {
                Id = 3,
                FullName = "Sam Miller",
                Email = "sam@example.com",
                PhoneNumber = "555-0303",
                DefaultShippingAddress = "88 Blues Road, Memphis",
                IsVip = false,
                CustomerLevel = "Regular"
            }
        };

        var carts = new List<Cart>
        {
            new Cart
            {
                Id = 1,
                CustomerId = 1,
                Subtotal = 0,
                DiscountAmount = 0,
                ShippingCost = 0,
                Total = 0
            },
            new Cart
            {
                Id = 2,
                CustomerId = 2,
                Subtotal = 0,
                DiscountAmount = 0,
                ShippingCost = 0,
                Total = 0
            },
            new Cart
            {
                Id = 3,
                CustomerId = 3,
                Subtotal = 0,
                DiscountAmount = 0,
                ShippingCost = 0,
                Total = 0
            }
        };

        var coupons = new List<Coupon>
        {
            new Coupon
            {
                Id = 1,
                Code = "RICK10",
                DiscountType = "Percentage",
                Value = 10,
                IsActive = true,
                MinimumOrderAmount = 500,
                VipOnly = false,
                ExpiresAt = DateTime.UtcNow.AddMonths(3)
            },
            new Coupon
            {
                Id = 2,
                Code = "VIP20",
                DiscountType = "Percentage",
                Value = 20,
                IsActive = true,
                MinimumOrderAmount = 1000,
                VipOnly = true,
                ExpiresAt = DateTime.UtcNow.AddMonths(3)
            },
            new Coupon
            {
                Id = 3,
                Code = "FREESHIP",
                DiscountType = "FreeShipping",
                Value = 0,
                IsActive = true,
                MinimumOrderAmount = 1500,
                VipOnly = false,
                ExpiresAt = DateTime.UtcNow.AddMonths(2)
            },
            new Coupon
            {
                Id = 4,
                Code = "OLDDEAL",
                DiscountType = "FixedAmount",
                Value = 100,
                IsActive = false,
                MinimumOrderAmount = 1000,
                VipOnly = false,
                ExpiresAt = DateTime.UtcNow.AddDays(-10)
            }
        };

        var reviews = new List<Review>
        {
            new Review
            {
                Id = 1,
                ProductId = 1,
                CustomerId = 1,
                Rating = 5,
                Comment = "Great tone and very comfortable to play."
            },
            new Review
            {
                Id = 2,
                ProductId = 2,
                CustomerId = 2,
                Rating = 5,
                Comment = "Heavy, powerful, and perfect for rock."
            },
            new Review
            {
                Id = 3,
                ProductId = 4,
                CustomerId = 3,
                Rating = 4,
                Comment = "Beautiful acoustic sound, but expensive."
            },
            new Review
            {
                Id = 4,
                ProductId = 9,
                CustomerId = 1,
                Rating = 5,
                Comment = "Reliable strings. I always buy these."
            }
        };

        db.Products.AddRange(products);
        db.Customers.AddRange(customers);
        db.Carts.AddRange(carts);
        db.Coupons.AddRange(coupons);
        db.Reviews.AddRange(reviews);

        db.SaveChanges();
    }
}