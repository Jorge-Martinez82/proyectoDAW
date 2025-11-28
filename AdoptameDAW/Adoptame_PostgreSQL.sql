-- ######################################
-- SCRIPT DE MIGRACIÓN DE SQL SERVER A POSTGRESQL
-- BASE DE DATOS: Adoptame
-- ######################################

-- 1. CREACIÓN DE TABLAS (DDL)

-- Tabla "Usuarios"
CREATE TABLE "Usuarios" (
	"Id" **SERIAL** NOT NULL,
	"Uuid" **UUID** NULL DEFAULT gen_random_uuid(),
	"Email" **TEXT** NOT NULL,
	"PasswordHash" **TEXT** NULL,
	"TipoUsuario" **TEXT** NOT NULL,
	"CreatedAt" **TIMESTAMP** NULL DEFAULT NOW(),
	PRIMARY KEY ("Id")
);

-- Tabla "Adoptantes"
CREATE TABLE "Adoptantes" (
	"Id" **SERIAL** NOT NULL,
	"Uuid" **UUID** NOT NULL DEFAULT gen_random_uuid(),
	"Nombre" **VARCHAR**(100) NOT NULL,
	"Apellidos" **VARCHAR**(200) NOT NULL,
	"Direccion" **VARCHAR**(255) NOT NULL,
	"CodigoPostal" **VARCHAR**(10) NOT NULL,
	"Poblacion" **VARCHAR**(100) NOT NULL,
	"Provincia" **VARCHAR**(100) NOT NULL,
	"Telefono" **VARCHAR**(20) NOT NULL,
	"Email" **VARCHAR**(255) NOT NULL,
	"UserId" **INT** NOT NULL,
	"CreatedAt" **TIMESTAMP** NOT NULL DEFAULT NOW(),
	PRIMARY KEY ("Id")
);

-- Tabla "Protectoras"
CREATE TABLE "Protectoras" (
	"Id" **SERIAL** NOT NULL,
	"Uuid" **UUID** NULL DEFAULT gen_random_uuid(),
	"Nombre" **TEXT** NOT NULL,
	"Direccion" **TEXT** NULL,
	"Telefono" **TEXT** NULL,
	"Provincia" **TEXT** NULL,
	"Email" **TEXT** NULL,
	"Imagen" **TEXT** NULL,
	"UserId" **INT** NOT NULL,
	"CreatedAt" **TIMESTAMP** NULL DEFAULT NOW(),
	PRIMARY KEY ("Id")
);

-- Tabla "Animales"
CREATE TABLE "Animales" (
	"Id" **SERIAL** NOT NULL,
	"Uuid" **UUID** NULL DEFAULT gen_random_uuid(),
	"Nombre" **TEXT** NOT NULL,
	"Tipo" **TEXT** NOT NULL,
	"Raza" **TEXT** NULL,
	"Edad" **INT** NULL,
	"Genero" **TEXT** NULL,
	"Descripcion" **TEXT** NULL,
	"ProtectoraId" **INT** NOT NULL,
	"ImagenPrincipal" **TEXT** NULL,
	"CreatedAt" **TIMESTAMP** NULL DEFAULT NOW(),
	PRIMARY KEY ("Id")
);

-- Tabla "Solicitudes"
CREATE TABLE "Solicitudes" (
	"id" **SERIAL** NOT NULL,
	"usuario_adoptante_id" **INT** NOT NULL,
	"usuario_protectora_id" **INT** NOT NULL,
	"animal_id" **INT** NOT NULL,
	"estado" **VARCHAR**(15) NOT NULL,
	"CreatedAt" **TIMESTAMP** NOT NULL DEFAULT NOW(),
	"comentario" **VARCHAR**(500) NULL,
	PRIMARY KEY ("id")
);

-- 2. ÍNDICES Y RESTRICCIONES ÚNICAS
ALTER TABLE "Adoptantes" ADD CONSTRAINT "UQ_Adoptantes_Email" UNIQUE ("Email");
ALTER TABLE "Adoptantes" ADD CONSTRAINT "UQ_Adoptantes_UserId" UNIQUE ("UserId");

-- 3. CLAVES FORÁNEAS (FOREIGN KEYS)
ALTER TABLE "Adoptantes" ADD CONSTRAINT "FK_Adoptantes_Usuarios" FOREIGN KEY("UserId")
REFERENCES "Usuarios" ("Id") ON DELETE CASCADE;

ALTER TABLE "Animales" ADD CONSTRAINT "FK_Animales_Protectoras" FOREIGN KEY("ProtectoraId")
REFERENCES "Protectoras" ("Id");

ALTER TABLE "Protectoras" ADD CONSTRAINT "FK_Protectoras_Usuarios" FOREIGN KEY("UserId")
REFERENCES "Usuarios" ("Id");

ALTER TABLE "Solicitudes" ADD CONSTRAINT "FK_Solicitudes_Animales" FOREIGN KEY("animal_id")
REFERENCES "Animales" ("Id");

ALTER TABLE "Solicitudes" ADD CONSTRAINT "FK_Solicitudes_Adoptante" FOREIGN KEY("usuario_adoptante_id")
REFERENCES "Usuarios" ("Id");

ALTER TABLE "Solicitudes" ADD CONSTRAINT "FK_Solicitudes_Protectora" FOREIGN KEY("usuario_protectora_id")
REFERENCES "Usuarios" ("Id");

-- 4. INSERCIÓN DE DATOS (DML)
-- NOTA: PostgreSQL inserta IDs secuenciales automáticamente con SERIAL.

-- **Usuarios** (Se insertan los IDs del 6 al 46)
INSERT INTO "Usuarios" ("Id", "Uuid", "Email", "PasswordHash", "TipoUsuario", "CreatedAt") OVERRIDING SYSTEM VALUE VALUES
(6, N'0a5712f5-3e72-421e-8cd3-f86d88185115', N'info@protectoravitoria.com', N'$2a$13$TPbIj0V98ZiAzirccN69gOtJHInd305XfTv/w/U5SLYFqgymTaGoa', N'Protectora', '2025-11-19 13:44:57.220'),
(7, N'262397ba-5dcc-4759-be23-c50770d91b58', N'contacto@patitasdonosti.com', N'$2a$13$TPbIj0V98ZiAzirccN69gOtJHInd305XfTv/w/U5SLYFqgymTaGoa', N'Protectora', '2025-11-19 13:45:14.157'),
(8, N'c8aee855-5944-42f2-b56f-717fb9f7b195', N'info@protectorabilbao.com', N'$2a$13$TPbIj0V98ZiAzirccN69gOtJHInd305XfTv/w/U5SLYFqgymTaGoa', N'Protectora', '2025-11-19 13:45:27.077'),
(9, N'ca5ff41c-c22d-4739-a906-9bff0eb3c064', N'contacto@refugiopamplona.com', N'$2a$13$TPbIj0V98ZiAzirccN69gOtJHInd305XfTv/w/U5SLYFqgymTaGoa', N'Protectora', '2025-11-19 13:45:35.963'),
(10, N'21a20cd9-1fda-4d42-a921-aa924234573c', N'info@protectoraeibar.com', N'$2a$13$TPbIj0V98ZiAzirccN69gOtJHInd305XfTv/w/U5SLYFqgymTaGoa', N'Protectora', '2025-11-19 13:45:43.440'),
(11, N'668b32a4-72f6-4a88-b9b9-a935a25443df', N'info@hogarbarakaldo.com', N'$2a$13$TPbIj0V98ZiAzirccN69gOtJHInd305XfTv/w/U5SLYFqgymTaGoa', N'Protectora', '2025-11-19 13:45:51.043'),
(12, N'4e9e9b3d-23b3-4142-baa3-78693e4a5f83', N'info@protectoraestella.com', N'$2a$13$TPbIj0V98ZiAzirccN69gOtJHInd305XfTv/w/U5SLYFqgymTaGoa', N'Protectora', '2025-11-19 13:46:00.073'),
(13, N'8ca15fb6-fd20-4f03-874e-dac58caf9ee2', N'contacto@protectoratafalla.com', N'$2a$13$TPbIj0V98ZiAzirccN69gOtJHInd305XfTv/w/U5SLYFqgymTaGoa', N'Protectora', '2025-11-19 13:46:09.233'),
(14, N'a052260b-3f1d-47bd-a7f3-f7da18e01242', N'info@refugiolaguardia.com', N'$2a$13$TPbIj0V98ZiAzirccN69gOtJHInd305XfTv/w/U5SLYFqgymTaGoa', N'Protectora', '2025-11-19 13:46:16.890'),
(46, N'1b02f1d8-fe8f-43dc-ac77-1df4e0ed505e', N'prueba@prueba.com', N'$2a$13$Qw9dPga/KfnI0JV9xvk.BO3SFfqYTRbBr1APHVrMA.gZZganxC./2', N'Adoptante', '2025-11-27 18:40:44.420');

-- **Adoptantes** (Se insertan los IDs del 25)
INSERT INTO "Adoptantes" ("Id", "Uuid", "Nombre", "Apellidos", "Direccion", "CodigoPostal", "Poblacion", "Provincia", "Telefono", "Email", "UserId", "CreatedAt") OVERRIDING SYSTEM VALUE VALUES
(25, N'78f5d730-f434-41b5-a0f7-81d89cb0a88b', N'Jorge', N'Martinez Moises', N'calle x', N'51169', N'Pamplona', N'Navarra', N'637232800', N'prueba@prueba.com', 46, '2025-11-27 18:40:44.427');

-- **Protectoras** (Se insertan los IDs del 1 al 10)
INSERT INTO "Protectoras" ("Id", "Uuid", "Nombre", "Direccion", "Telefono", "Provincia", "Email", "Imagen", "UserId", "CreatedAt") OVERRIDING SYSTEM VALUE VALUES
(1, N'fa118722-b32d-472e-b391-9831a3b45485', N'Protectora Animales Vitoria', N'Calle Mayor 1, Vitoria-Gasteiz', N'945123456', N'Álava', N'info@protectoravitoria.com', N'https://img.icons8.com/?size=100&id=UKqQTR3eQ8Mx&format=png&color=000000', 6, '2025-11-07 10:32:46.140'),
(2, N'041f7842-3ae3-432b-9d52-db3766969616', N'Refugio Patitas Donosti', N'Avenida Libertad 45, San Sebastián', N'943234567', N'Guipúzcoa', N'contacto@patitasdonosti.com', N'https://img.icons8.com/?size=100&id=72721&format=png&color=000000', 7, '2025-11-07 10:32:46.140'),
(3, N'723688a8-c9b1-4845-9b3b-b67d4edbf904', N'Asociación Protectora Bilbao', N'Gran Vía 78, Bilbao', N'944345678', N'Vizcaya', N'info@protectorabilbao.com', N'https://img.icons8.com/?size=100&id=KHaTirx3bZDw&format=png&color=000000', 8, '2025-11-07 10:32:46.140'),
(4, N'9d70cd03-e05d-4a44-9780-2e399cdf0f62', N'Refugio Animal Pamplona', N'Calle Amaya 23, Pamplona', N'948456789', N'Navarra', N'contacto@refugiopamplona.com', N'https://img.icons8.com/?size=100&id=A5wgSIpJYCuT&format=png&color=000000', 9, '2025-11-07 10:32:46.140'),
(5, N'5393093b-0568-4c20-a6f3-1a6bbb76b6a3', N'Protectora Animalista Eibar', N'Plaza Unzaga 5, Eibar', N'943567890', N'Guipúzcoa', N'info@protectoraeibar.com', N'https://img.icons8.com/?size=100&id=i7JWmvis54FG&format=png&color=000000', 10, '2025-11-07 10:32:46.140'),
(6, N'9aa6b17b-2d10-4641-a707-df192dd21033', N'Hogar Animal Barakaldo', N'Calle Herriko 34, Barakaldo', N'944678901', N'Vizcaya', N'info@hogarbarakaldo.com', N'https://img.icons8.com/?size=100&id=OJescyBSyxa0&format=png&color=000000', 11, '2025-11-07 10:32:46.140'),
(8, N'a5f6d976-ce85-4bfd-a5fa-ec0e0a1ee0e9', N'Protectora Animal Estella', N'Calle Baja Navarra 15, Estella', N'948556123', N'Navarra', N'info@protectoraestella.com', N'https://img.icons8.com/?size=100&id=AFolCzaOyeXO&format=png&color=000000', 12, '2025-11-13 15:06:02.020'),
(9, N'082d0533-3a11-4a65-a7bf-8d5457e3eef9', N'Asociación Protectora Tafalla', N'Avenida de Zaragoza 45, Tafalla', N'948702456', N'Navarra', N'contacto@protectoratafalla.com', N'https://img.icons8.com/?size=100&id=123849&format=png&color=000000', 13, '2025-11-13 15:06:02.020'),
(10, N'6db78b96-49f3-4502-8693-c6ac43a8e6bd', N'Refugio Animal Laguardia', N'Camino de la Hoya 8, Laguardia', N'945621089', N'Álava', N'info@refugiolaguardia.com', N'https://img.icons8.com/?size=100&id=Y44CCAKbsyuu&format=png&color=000000', 14, '2025-11-13 15:06:02.020');

-- **Animales** (Se insertan los IDs del 1 al 30)
INSERT INTO "Animales" ("Id", "Uuid", "Nombre", "Tipo", "Raza", "Edad", "Genero", "Descripcion", "ProtectoraId", "ImagenPrincipal", "CreatedAt") OVERRIDING SYSTEM VALUE VALUES
(1, N'9766d697-96ad-4a73-9b9b-54a4aa0eef1f', N'Max', N'Perro', N'Labrador', 3, N'Macho', N'Perro muy cariñoso y juguetón, ideal para familias. Disfruta de largos paseos, le encanta jugar con los niños y se adapta fácilmente a diferentes ambientes. Busca un hogar donde pueda recibir atención y mucho cariño, perfecto para quienes desean un compañero activo y leal.', 1, N'https://images.dog.ceo/breeds/labrador/n02099712_2668.jpg', '2025-11-07 10:32:46.140'),
(2, N'51bab786-54f2-48e9-a946-29a120b5674f', N'Luna', N'Perro', N'Golden Retriever', 2, N'Hembra', N'Perra tranquila y obediente, perfecta para niños. Sabe comportarse en casa, es inteligente y aprende rápido. Ideal para familias que buscan una mascota sociable y cariñosa. Le gusta acurrucarse en el sofá y recibir caricias, necesita un hogar donde la valoren y le brinden estabilidad.', 1, N'https://images.dog.ceo/breeds/retriever-golden/sasha2_delgado.jpg', '2025-11-07 10:32:46.140'),
(3, N'56ff9369-71e7-4978-b6a6-57f4082675ce', N'Rocky', N'Perro', N'Pastor Alemán', 4, N'Macho', N'Perro guardián leal y protector. Siempre alerta y dispuesto a cuidar su entorno, es un gran amigo para el ejercicio al aire libre. Necesita espacio para correr y jugar, y una familia que lo incluya en sus actividades diarias. Es afectuoso y fiel, excelente para quienes buscan seguridad y compañía.', 2, N'https://images.dog.ceo/breeds/german-shepherd/n02106662_16014.jpg', '2025-11-07 10:32:46.140'),
(4, N'a33cf14d-f7b6-42af-b7e7-3a302111fba3', N'Bella', N'Perro', N'Mestizo', 1, N'Hembra', N'Cachorrita activa buscando hogar amoroso. Juguetona y curiosa, disfruta de descubrir nuevos lugares y aprender cosas. Tiene mucha energía y vitalidad, ideal para personas dispuestas a dedicarle tiempo y paciencia. Será una mascota maravillosa si recibe educación y socialización adecuadas.', 2, N'https://images.dog.ceo/breeds/appenzeller/n02107908_2272.jpg', '2025-11-07 10:32:46.140'),
(5, N'a2634489-5727-474c-868a-a65e1434694a', N'Bruno', N'Perro', N'Boxer', 5, N'Macho', N'Perro enérgico que necesita mucho ejercicio. Sociable con otros perros y personas, disfruta de actividades al aire libre. Perfecto para familias deportivas o personas que busquen salir a caminar o correr con una mascota. Aprecia la compañía constante y los juegos interactivos.', 3, N'https://images.dog.ceo/breeds/boxer/n02108089_959.jpg', '2025-11-07 10:32:46.140'),
(6, N'c713aa09-2567-44d8-89b2-616ea3999ab5', N'Lola', N'Perro', N'Chihuahua', 3, N'Hembra', N'Perra pequeña pero con gran personalidad, siempre dispuesta a divertir y compartir buenos momentos con su familia. Requiere compañía y afecto, responde muy bien a la educación y le encanta jugar con niños. Es perfecta para hogares acogedores que quieren dar amor.', 3, N'https://images.dog.ceo/breeds/chihuahua/n02085620_3928.jpg', '2025-11-07 10:32:46.140'),
(7, N'356e6e88-b12a-4613-ae76-f769fbf9ba89', N'Thor', N'Perro', N'Rottweiler', 6, N'Macho', N'Perro grande y poderoso, muy leal a su familia. Requiere atención, ejercicio y socialización constante. Es excelente guardián, pero también muestra ternura y devoción a quienes confían en él. Ideal para personas con experiencia y tiempo para dedicarle.', 4, N'https://images.dog.ceo/breeds/rottweiler/n02106550_1260.jpg', '2025-11-07 10:32:46.140'),
(8, N'208ad40a-97ba-4eae-bf64-dacd0eb324f1', N'Kira', N'Perro', N'Border Collie', 2, N'Hembra', N'Perra inteligente y activa que necesita estimulación mental y física. Se lleva bien con otros animales y disfruta de juegos interactivos. Requiere atención diaria para estar feliz. Ideal para una familia comprometida con el bienestar animal que valore su energía.', 4, N'https://images.dog.ceo/breeds/collie-border/n02106166_152.jpg', '2025-11-07 10:32:46.140'),
(9, N'63710b15-9c81-4cbc-8ac6-e9c2a74372e6', N'Toby', N'Perro', N'Beagle', 4, N'Macho', N'Perro alegre y sociable con otros animales, le encanta salir de paseo y conocer gente nueva. Necesita un hogar estable que le motive y cuide. Responde bien al adiestramiento y demuestra gran fidelidad, siempre dispuesto a aprender y formar parte de la familia.', 5, N'https://images.dog.ceo/breeds/beagle/n02088364_15305.jpg', '2025-11-07 10:32:46.140'),
(10, N'94056a25-2804-466a-9f13-dc9b8be3bdfd', N'Nala', N'Perro', N'Mestizo', 3, N'Hembra', N'Perra cariñosa rescatada de la calle, agradecida y lista para encontrar una familia que le ofrezca una segunda oportunidad. Es juguetona y aprende rápido, ideal para hogares tranquilos y amables, siempre buscando cariño y protección.', 5, N'https://images.dog.ceo/breeds/ovcharka-caucasian/IMG_20200101_000620.jpg', '2025-11-07 10:32:46.140'),
(11, N'a2b120c9-d10e-4c3a-8bc8-adff9b0082a7', N'Rex', N'Perro', N'Husky Siberiano', 2, N'Macho', N'Perro hermoso y enérgico que ama correr y jugar en espacios abiertos, sociable con toda la familia y con otros animales. Requiere ejercicio frecuente y mucho cariño. Será un compañero ideal para quienes disfrutan del deporte y actividades externas.', 10, N'https://images.dog.ceo/breeds/husky/n02110185_9396.jpg', '2025-11-07 10:32:46.140'),
(12, N'5d0638bc-5fad-47f1-8257-a634c741b9ea', N'Mia', N'Perro', N'Poodle', 7, N'Hembra', N'Perra mayor muy tranquila y cariñosa, ideal para casas tranquilas y familias que buscan compañía serena. Es leal y sensible, disfruta de largos ratos de calma y afecto. Busca un hogar que valore su experiencia y le brinde un retiro digno y cómodo.', 6, N'https://images.dog.ceo/breeds/poodle-standard/n02113799_7121.jpg', '2025-11-07 10:32:46.140'),
(13, N'80b84af5-dc40-46fe-8191-6a8261ace28f', N'Simba', N'Gato', N'Naranja', 2, N'Macho', N'Gata juguetona y curiosa, siempre en busca de aventuras y exploraciones. Se adapta fácilmente a nuevos ambientes, le encanta recibir caricias y demostrar su afecto. Es ideal para quienes buscan una mascota activa y entretenida.', 1, N'https://cdn2.thecatapi.com/images/gt.jpg', '2025-11-07 10:32:46.140'),
(14, N'fe1529fe-8301-4500-8742-aa571b8c3c50', N'Minina', N'Gato', N'Siamés', 3, N'Hembra', N'Gata elegante y vocal, con gran necesidad de atención. Le gustan los lugares cómodos y la compañía constante del humano, disfruta de ser el centro de atención y hacer sentir especial a su cuidador.', 1, N'https://cdn2.thecatapi.com/images/FTd8l4EXq.jpg', '2025-11-07 10:32:46.140'),
(15, N'a2bc4862-1aa2-4481-8ecd-f3e8cb4af69f', N'Pelusa', N'Gato', N'Persa', 4, N'Hembra', N'Gata tranquila que adora las caricias y los momentos de paz en el hogar. Se adapta bien a distintos tipos de familias y puede convivir con otras mascotas, es ideal para quienes buscan serenidad y afecto en su día a día.', 2, N'https://cdn2.thecatapi.com/images/e0j.jpg', '2025-11-07 10:32:46.140'),
(16, N'd34b8d36-f840-4d1c-a0b4-83800e2d16e6', N'Garfield', N'Gato', N'Europeo', 5, N'Macho', N'Gato grande y precioso, muy simpático y sociable, curioso y con ganas de descubrir cada rincón de la casa. Aportará alegría y cariño a quien lo adopte, siendo el compañero ideal para compartir momentos únicos y experiencias nuevas.', 10, N'https://cdn2.thecatapi.com/images/MTc0MzI1OA.png', '2025-11-07 10:32:46.140'),
(17, N'e4d4f22c-9243-4463-bac0-89c4d086e660', N'Nieve', N'Gato', N'Angora', 1, N'Hembra', N'Gatita blanca muy juguetona, siempre lista para divertirse y recibir cariño. Es dulce y activa, perfecta para quienes buscan un animal que alegre el hogar y brinde amor a toda la familia.', 3, N'https://cdn2.thecatapi.com/images/1he.jpg', '2025-11-07 10:32:46.140'),
(18, N'f36938eb-2453-4170-910d-a997125724a2', N'Tigre', N'Gato', N'Atigrado', 3, N'Macho', N'Gato cazador pero muy cariñoso con humanos, disfruta de pasar tiempo en jardines y espacios abiertos. Es independiente y hábil, pero siempre busca el momento para recibir caricias y compañía especial.', 3, N'https://cdn2.thecatapi.com/images/a70.jpg', '2025-11-07 10:32:46.140'),
(19, N'59422160-85d4-4638-8aaa-c0f7ee9f9d0a', N'Luna', N'Gato', N'Negro', 2, N'Hembra', N'Gata misteriosa y elegante, silenciosa y observadora, perfecta para hogares tranquilos que sepan apreciar su belleza y carácter especial. Busca una familia que le dedique atención y cariño individual.', 9, N'https://cdn2.thecatapi.com/images/bsf.jpg', '2025-11-07 10:32:46.140'),
(20, N'05faf0d9-3021-4e6d-b590-f43f7f233e36', N'Félix', N'Gato', N'Mestizo', 6, N'Macho', N'Perro juguetón y muy activo, le encanta correr y seguir pelotas por todo el parque. Es sociable con otros animales y se adapta a cualquier tipo de familia, ideal como compañero para niños y adultos aventureros.', 4, N'https://cdn2.thecatapi.com/images/di1.jpg', '2025-11-07 10:32:46.140'),
(21, N'87b282e4-c8f6-4cdb-bf94-5ebfb922bbb0', N'Cleo', N'Gato', N'Carey', 1, N'Hembra', N'Gata mimosona y tranquila, disfruta dormir junto a su dueño y recibir caricias en cualquier momento. Es dócil, buena con niños y sabe comportarse perfectamente en ambientes familiares.', 5, N'https://cdn2.thecatapi.com/images/eha.jpg', '2025-11-07 10:32:46.140'),
(22, N'08a9288d-3d0c-492b-b368-54618fb48022', N'Silvestre', N'Gato', N'Blanco y Negro', 4, N'Macho', N'Perro curioso, siempre dispuesto a explorar y aprender cosas nuevas. Le gusta estar rodeado de gente y participar en actividades grupales, es muy adaptable y cariñoso con todos.', 5, N'https://cdn2.thecatapi.com/images/ei3.jpg', '2025-11-07 10:32:46.140'),
(23, N'716d535a-816c-4481-8743-47d8b4e9f8a8', N'Kitty', N'Gato', N'Himalayo', 3, N'Hembra', N'Gato de gran personalidad, independiente pero fiel a su familia. Disfruta de los ratos de juego y también de los espacios tranquilos para descansar. Busca un hogar donde pueda ser él mismo y brindar compañía discreta.', 6, N'https://cdn2.thecatapi.com/images/J2PmlIizw.jpg', '2025-11-07 10:32:46.140'),
(24, N'be76d918-09af-4f18-b716-8d342f93f2a5', N'Tom', N'Gato', N'Gris', 5, N'Macho', N'Perro energético y amigable, perfecto para actividades al aire libre y familias con niños. Aprende rápido, le gusta el adiestramiento y responde bien a las normas básicas. Muy afectuoso y atento a sus humanos.', 9, N'https://cdn2.thecatapi.com/images/PoZIVJ124.jpg', '2025-11-07 10:32:46.140'),
(25, N'aab443f2-6cf9-4129-afce-2b8e90d4e18f', N'Chip', N'Roedor', N'Hámster Dorado', 1, N'Macho', N'Gata curiosa y activa, siempre pendiente de cada rincón y objeto nuevo en casa. Es simpática y sabe adaptarse tanto a ambientes tranquilos como a familias bulliciosas.', 1, N'https://images.pexels.com/photos/10884423/pexels-photo-10884423.jpeg?_gl=1*f7cv4n*_ga*MTE0NDU3MTI1Mi4xNzYzMDIzODgw*_ga_8JE65Q40S6*czE3NjMwMjM4ODAkbzEkZzEkdDE3NjMwMjM4ODYkajU0JGwwJGgw', '2025-11-07 10:32:46.140'),
(26, N'1bad8357-3246-4b8c-aa05-c6f700800a05', N'Coco', N'Roedor', N'Cobaya', 2, N'Hembra', N'Perro noble y leal, adora proteger a su familia y participar en juegos de grupo. Busca un hogar estable donde pueda sentirse útil y querido, será un amigo fiel durante toda su vida.', 3, N'https://images.pexels.com/photos/34676096/pexels-photo-34676096.jpeg?_gl=1*iyjafd*_ga*MTE0NDU3MTI1Mi4xNzYzMDIzODgw*_ga_8JE65Q40S6*czE3NjMwMjM4ODAkbzEkZzEkdDE3NjMwMjM5NTAkajU5JGwwJGgw', '2025-11-07 10:32:46.140'),
(27, N'cbf5bb00-457f-46fa-b363-aeecbd39e5b6', N'Ratoncito', N'Roedor', N'Ratón Doméstico', 1, N'Macho', N'Gata elegante y reservada, ideal para personas que buscan tranquilidad y calma en el hogar. Sabe dónde encontrar su lugar favorito para descansar y pasar largos ratos observando el exterior.', 8, N'https://images.pexels.com/photos/53813/pexels-photo-53813.jpeg?_gl=1*2do9s1*_ga*MTE0NDU3MTI1Mi4xNzYzMDIzODgw*_ga_8JE65Q40S6*czE3NjMwMjM4ODAkbzEkZzEkdDE3NjMwMjQ3MzEkajU5JGwwJGgw', '2025-11-07 10:32:46.140'),
(28, N'929cb255-b2a5-4a56-80e4-afad84adf272', N'Peluchín', N'Roedor', N'Conejo Enano', 3, N'Macho', N'Perro cariñoso con mucha energía, disfruta de las caminatas y los juegos en espacio abierto. Es muy social, apto para convivir con niños y otras mascotas, siempre dispuesto a recibir cariño.', 6, N'https://images.pexels.com/photos/372166/pexels-photo-372166.jpeg?_gl=1*vfa2ns*_ga*MTE0NDU3MTI1Mi4xNzYzMDIzODgw*_ga_8JE65Q40S6*czE3NjMwMjM4ODAkbzEkZzEkdDE3NjMwMjQ2ODIkajMwJGwwJGgw', '2025-11-07 10:32:46.140'),
(29, N'56f6f6d8-dd74-4171-a9c8-addc093aff50', N'Pepe', N'Reptil', N'Tortuga de Tierra', 10, N'Macho', N'Gata dulce y sociable, le encanta recibir atención y compartir momentos relajados con su familia humana. Se lleva bien con otros gatos y puede adaptarse a cualquier ambiente cálido y afectuoso.', 2, N'https://images.pexels.com/photos/3753182/pexels-photo-3753182.jpeg?_gl=1*1pvjq3d*_ga*MTE0NDU3MTI1Mi4xNzYzMDIzODgw*_ga_8JE65Q40S6*czE3NjMwMjM4ODAkbzEkZzEkdDE3NjMwMjQ4NjgkajU5JGwwJGgw', '2025-11-07 10:32:46.140'),
(30, N'f1a24859-3e56-47d2-86dc-03700582c4e0', N'Verde', N'Reptil', N'Iguana', 5, N'Hembra', N'Perro protector pero amable, agradecido y tranquilo. Perfecto para hogares con personas mayores o ambientes familiares serenos, aporta seguridad y equilibrio afectivo.', 8, N'https://images.pexels.com/photos/22608246/pexels-photo-22608246.jpeg?_gl=1*gb06ha*_ga*MTE0NDU3MTI1Mi4xNzYzMDIzODgw*_ga_8JE65Q40S6*czE3NjMwMjM4ODAkbzEkZzEkdDE3NjMwMjQ4MDMkajU5JGwwJGgw', '2025-11-07 10:32:46.140');

-- 5. ACTUALIZAR LAS SECUENCIAS
-- Esta sección es crucial para que los nuevos INSERTs sigan la numeración de los datos migrados.
SELECT setval(pg_get_serial_sequence('"Usuarios"', 'Id'), (SELECT MAX("Id") FROM "Usuarios"), true);
SELECT setval(pg_get_serial_sequence('"Adoptantes"', 'Id'), (SELECT MAX("Id") FROM "Adoptantes"), true);
SELECT setval(pg_get_serial_sequence('"Protectoras"', 'Id'), (SELECT MAX("Id") FROM "Protectoras"), true);
SELECT setval(pg_get_serial_sequence('"Animales"', 'Id'), (SELECT MAX("Id") FROM "Animales"), true);
SELECT setval(pg_get_serial_sequence('"Solicitudes"', 'id'), (SELECT MAX("id") FROM "Solicitudes"), true);