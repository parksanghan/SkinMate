import cv2
import mediapipe as mp

mp_face_mesh = mp.solutions.face_mesh

LEFT_EYE_BOTTOM = 145
RIGHT_EYE_BOTTOM = 374
LEFT_LIP_CORNER = 61
RIGHT_LIP_CORNER = 291

BOX_WIDTH = 50
BOX_HEIGHT = 50

cap = cv2.VideoCapture(0)

with mp_face_mesh.FaceMesh(
    static_image_mode=False,
    max_num_faces=1,
    refine_landmarks=True,
    min_detection_confidence=0.5,
) as face_mesh:
    while cap.isOpened():
        success, frame = cap.read()
        if not success:
            break

        image = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        results = face_mesh.process(image)
        image = cv2.cvtColor(image, cv2.COLOR_RGB2BGR)
        h, w, _ = image.shape

        if results.multi_face_landmarks:
            for face_landmarks in results.multi_face_landmarks:
                # 왼쪽 볼 중간점 (눈 아래 ↔ 입꼬리 중간)
                lx1 = int(face_landmarks.landmark[LEFT_EYE_BOTTOM].x * w)
                ly1 = int(face_landmarks.landmark[LEFT_EYE_BOTTOM].y * h)
                lx2 = int(face_landmarks.landmark[LEFT_LIP_CORNER].x * w)
                ly2 = int(face_landmarks.landmark[LEFT_LIP_CORNER].y * h)
                lcx = (lx1 + lx2) // 2
                lcy = (ly1 + ly2) // 2

                # 오른쪽 볼 중간점
                rx1 = int(face_landmarks.landmark[RIGHT_EYE_BOTTOM].x * w)
                ry1 = int(face_landmarks.landmark[RIGHT_EYE_BOTTOM].y * h)
                rx2 = int(face_landmarks.landmark[RIGHT_LIP_CORNER].x * w)
                ry2 = int(face_landmarks.landmark[RIGHT_LIP_CORNER].y * h)
                rcx = (rx1 + rx2) // 2
                rcy = (ry1 + ry2) // 2

                # 바운딩 박스
                cv2.rectangle(
                    image,
                    (lcx - BOX_WIDTH // 2, lcy - BOX_HEIGHT // 2),
                    (lcx + BOX_WIDTH // 2, lcy + BOX_HEIGHT // 2),
                    (0, 255, 0),
                    2,
                )
                cv2.rectangle(
                    image,
                    (rcx - BOX_WIDTH // 2, rcy - BOX_HEIGHT // 2),
                    (rcx + BOX_WIDTH // 2, rcy + BOX_HEIGHT // 2),
                    (255, 0, 0),
                    2,
                )

                # 디버깅 점
                cv2.circle(image, (lcx, lcy), 2, (0, 0, 255), -1)
                cv2.circle(image, (rcx, rcy), 2, (0, 0, 255), -1)

        cv2.imshow("Refined Cheek Region Detection", image)
        if cv2.waitKey(5) & 0xFF == 27:
            break

cap.release()
cv2.destroyAllWindows()
