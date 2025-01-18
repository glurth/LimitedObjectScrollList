# LimitedObjectScrollList Package

## Overview

The **LimitedObjectScrollList** package is an optimization utility for managing and displaying extensive lists within Unity's UI framework. It enables efficient rendering of large datasets by using a limited number of instantiated UI objects, proportional to the viewport size. This approach minimizes performance overhead and improves scalability in your Unity projects.

This package includes generic base classes for custom UI implementations and ready-to-use prefab templates to jumpstart development. Uses the IDisplay<T> interface to allow the user to make custom list elements.

---

## Features

- **Efficient Object Management**: Dynamically instantiate and reuse a minimal number of UI elements, regardless of the list's total size.
- **Customizable UI**: Extend the provided generic classes to create tailored list views for your specific project requirements.
- **Event Handling**: Built-in support for common UI interactions such as selection, hover, clicks, and value changes.
- **Extensibility**: Use prefabs that implement required interfaces to handle advanced UI events.
- **Templates**: Ready-to-use prefabs for quick integration of read-only and editable text lists.
- **Viewport Adaptation**: Automatically adjusts the displayed elements based on the visible area, optimizing performance.

---

## Installation

1. Clone or download the package from the repository.
2. Import the package into your Unity project via the Unity Editor:
   - `Assets > Import Package > Custom Package`.
3. Add the necessary prefabs or scripts to your Unity scene.

---

## Getting Started

### Setting Up the Scroll List

1. **Add the Component**:
   - Attach the `LimitedObjectScrollList` component to a GameObject in your scene.
   - Ensure the GameObject has a `ScrollRect` component.

2. **Configure the Prefab**:
   - Assign a prefab to the `lineElementPreFab` field in the component's Inspector.
   - The prefab must implement the `IDisplay<TListElementType>` interface to function correctly.

3. **Specify List Data**:
   - Use the `SetList` method to pass your data to the scroll list.

### Example Code

Here is an example of setting up and populating the scroll list programmatically:

```csharp
using System.Collections.Generic;
using UnityEngine;
using EyE.Unity.UI;

public class ExampleUsage : MonoBehaviour
{
    public LimitedObjectScrollList<string, MyLineElement> scrollList;

    void Start()
    {
        List<string> data = new List<string> { "Item 1", "Item 2", "Item 3" };
        scrollList.SetList(data);
    }
}
```

---

## Core Components

### `LimitedObjectScrollList`
A generic class that manages the scrollable list. It requires two type parameters:

- **`TListElementType`**: The data type of elements in the list.
- **`TLineElementPreFabType`**: The prefab type used to display each list element. It must:
  - Implement `IDisplay<TListElementType>`.
  - Inherit from `MonoBehaviour`.

#### Key Properties
- **`lineElementPreFab`**: The prefab used to display list elements.
- **`lineElementHeight`**: The height of each line element.
- **`controllerScrollRect`**: The `ScrollRect` component controlling the list.

#### Key Methods
- **`SetList(List<TListElementType>)`**: Populates the list with data.
- **`GetDisplayElement(int)`**: Retrieves the displayed element for a given index, if visible.

### Events
- **`onSelectEvent`**: Triggered when an element is selected.
- **`onClickEvent`**: Triggered when an element is clicked.
- **`onPointerEnterEvent` and `onPointerExitEvent`**: Triggered when the pointer enters or exits an element.
- **`onValueChangedEvent`**: Triggered when an element's value changes.
- **`onValueEditEndEvent`**: Triggered when the user finishes editing an element's value.
- **`onElementInView`**: Triggered when an element becomes visible in the viewport.

---

## Prefab Requirements

### Interfaces
To enable various events, the prefab must implement one or more of the following interfaces:

- **`IDisplay<TListElementType>`**: Displays data of type `TListElementType`.
- **`ITriggerOnSelect`**: Supports selection events.
- **`ITriggerOnHover`**: Supports hover events.
- **`ITriggerOnClick`**: Supports click events.
- **`ITriggerOnValueChange<T>`**: Supports value change events.
- **`ITriggerOnValueEditEnd<T>`**: Supports end-of-edit events.

### Example Prefab Setup
1. Create a prefab with the necessary components.
2. Implement the required interfaces in the prefab's scripts.
3. Assign the prefab to the `lineElementPreFab` field in the `LimitedObjectScrollList` component.

---

## Performance Tips

- Optimize prefab complexity to minimize instantiation overhead.
- Set `lineElementHeight` accurately to match your prefab's height.
- Use a fixed number of instantiated elements proportional to the viewport height.

---

## Known Limitations

- Prefab setup requires implementing specific interfaces.
- The system assumes consistent element heights.
- Scroll synchronization is limited to `ScrollRect` components.

---

## Contributions

Contributions are welcome! Feel free to submit issues, feature requests, or pull requests.

---

## License

No license is provided without written permission. (Free for indy devs, just ask. [glurth at gmail dot com])

---



