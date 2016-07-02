# Pretzel.Picture

Allows you to wrap you `<img>` tags with `<figure>` and add `<figcaption>`.

This is a plugin for the static site generation tool [Pretzel](https://github.com/Code52/pretzel).

### Usage

To add properly formatted `<img>` tag to the page:

```html
{% picture "/assets/images/darkside.jpg" %}
Image of dark side
<cite>— Photo by <a href="http://zwezdin.com" rel="author" target="_blank">Sergey Zwezdin</a></cite>
{% endpicture %}
```

You can also customize it by adding some CSS-classes:

```html
{% picture "/assets/images/darkside.jpg" "bordered" %}
Image of dark side
<cite>— Photo by <a href="http://zwezdin.com" rel="author" target="_blank">Sergey Zwezdin</a></cite>
{% endpicture %}
```

You can skip any descriptions for image. In this case `<figcaption>` won't be added.

```html
{% picture "/assets/images/darkside.jpg" %}
{% endpicture %}
```

### Installation

Download the [latest release](https://github.com/sergeyzwezdin/Pretzel.Picture/releases/latest) and copy `Pretzel.Picture.dll` to the `_plugins` folder at the root of your site folder.